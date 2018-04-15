using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Win32;
using System.Drawing;

namespace SpotiREC
{
    public partial class Main : Form
    {
        private SettingsForm settingsForm;

        public static string logPath = Application.StartupPath + @"\SpotiREC_log.txt";
        private string savePath = "";
        private string libavPath = @"C:\youdl\";
        private string lastArtistName = "";
        private string lastTrackName = "";
        System.Timers.Timer timer;
        bool recordOnPlay = false;

        // CSCore Variables
        private WasapiCapture _soundIn;
        private IWriteable _writer;
        private readonly GraphVisualization _graphVisualization = new GraphVisualization();
        private IWaveSource _finalSource;

        public Main()
        {
            InitializeComponent();

            settingsForm = new SettingsForm();

            renderTransparent(LB_Status);
            renderTransparent(LB_Position);
            renderTransparent(BT_Record);
            renderTransparent(BT_RecordOnPlay);
            renderTransparent(BT_Stop);
            renderTransparent(BT_Convert);
            renderTransparent(BT_Settings);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            ToolTip tt1 = new ToolTip();
            tt1.SetToolTip(BT_RecordOnPlay, "Smart Spotify audio recording");
            tt1.SetToolTip(BT_Record, "Record sound from selected device");
            tt1.SetToolTip(BT_Convert, "Convert all files from the selected directory from WAV to MP3");
            tt1.SetToolTip(BT_Stop, "Stop the recording");

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += MainTimer_Tick;
            timer.Start();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Process.GetProcessesByName("spotify").Length < 1)
                {
                    File.AppendAllText(logPath, "Spotify process not found\r\n");
                    if (LB_Status.InvokeRequired)
                        LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = "Spotify non démarré"; }));
                    return;
                    //Application.Exit();
                }

                WebHelperResult whr = WebHelperHook.GetStatus();

                if (whr.isAd) // Track is ad
                {
                    if (whr.isPlaying)
                    {
                        Debug.WriteLine("Ad is playing");
                        if (lastArtistName != whr.artistName)
                        {
                            lastArtistName = whr.artistName;
                            //LogAction("/mute/" + whr.artistName);
                            Debug.WriteLine("Blocked " + whr.artistName);
                        }
                    }
                    else // Ad is paused
                        Debug.WriteLine("Ad is paused");
                }
                else if (whr.isPrivateSession)
                {
                    if (lastArtistName != whr.artistName)
                    {
                        lastArtistName = whr.artistName;
                        MessageBox.Show("Please disable 'Private Session' on Spotify for EasyBlocker to function properly.", "EasyBlocker", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                }
                else if (!whr.isRunning)
                {
                    if (LB_Status.InvokeRequired)
                        LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = "SpotifyWebHelper non démarré"; }));
                    File.AppendAllText(logPath, "Not running.\r\n");
                    MainTimer.Interval = 5000;
                }
                else if (!whr.isPlaying)
                {
                    if (recordOnPlay)
                    {
                        StopCapture();
                        recordOnPlay = false;
                    }

                    if (LB_Status.InvokeRequired)
                        LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = "Spotify en pause"; }));
                    lastArtistName = "";
                    lastTrackName = "";
                }
                else // Song is playing
                {          
                    if (whr.length - whr.position < 3) timer.Interval = 300;
                    else if (MainTimer.Interval != 1000) timer.Interval = 1000;

                    if (lastArtistName != whr.artistName || lastTrackName != whr.trackName)
                    {
                        // Delegate refresh on track change
                        if (recordOnPlay)
                        {
                            StopCapture();
                            StartCapture(savePath + "\\" + strToFile(whr.artistName + " - " + whr.trackName + ".wav"));
                        }

                        if (LB_Status.InvokeRequired)
                            LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = ShortenName("> " + whr.artistName + " - " + whr.trackName); }));
                        if (PlayingProgress.InvokeRequired)
                            PlayingProgress.Invoke(new MethodInvoker(delegate { PlayingProgress.Maximum = whr.length; }));
                        lastArtistName = whr.artistName;
                        lastTrackName = whr.trackName;
                    }

                    if (LB_Position.InvokeRequired)
                        LB_Position.Invoke(new MethodInvoker(delegate { LB_Position.Text = toTimeString(whr.position, whr.length); }));
                    Int32 minValue = (int)whr.position < whr.length ? (int)whr.position : whr.length;
                    if (PlayingProgress.InvokeRequired)
                        PlayingProgress.Invoke(new MethodInvoker(delegate { PlayingProgress.Value = minValue; }));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                File.AppendAllText(logPath, ex.Message + '\n');
            }
        }

        private void StartCapture(string fileName)
        {
            if (settingsForm.SelectedDevice == null)
                return;

            _soundIn = new WasapiLoopbackCapture();
            _soundIn.Device = settingsForm.SelectedDevice;
            _soundIn.Initialize();

            // Check file name availability
            string finalName = Path.ChangeExtension(fileName, null);
            while (File.Exists(finalName + ".wav"))
                finalName += "_";
            finalName += ".wav";

            var soundInSource = new SoundInSource(_soundIn);
            var singleBlockNotificationStream = new SingleBlockNotificationStream(soundInSource.ToSampleSource());
            _finalSource = singleBlockNotificationStream.ToWaveSource();
            _writer = new WaveWriter(finalName, _finalSource.WaveFormat);

            byte[] buffer = new byte[_finalSource.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, e) =>
            {
                int read;
                while ((read = _finalSource.Read(buffer, 0, buffer.Length)) > 0)
                    _writer.Write(buffer, 0, read);
            };

            singleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStreamOnSingleBlockRead;

            _soundIn.Start();
        }

        private void SingleBlockNotificationStreamOnSingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            _graphVisualization.AddSamples(e.Left, e.Right);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "WAV (*.wav)|*.wav",
                Title = "Save",
                FileName = lastArtistName + " - " + lastTrackName
            };
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                StartCapture(sfd.FileName);
                BT_Record.Enabled = false;
                BT_Stop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCapture();
        }

        private void StopCapture()
        {
            if (_soundIn != null)
            {
                _soundIn.Stop();
                _soundIn.Dispose();
                _soundIn = null;
                _finalSource.Dispose();

                if (_writer is IDisposable)
                    ((IDisposable)_writer).Dispose();

                if (!recordOnPlay)
                {
                    BT_Stop.Enabled = false;
                    BT_Record.Enabled = true;
                }
            }
        }

        private string strToFile(string fileName)
        {
            string temp = fileName;
            string[] charsToRemove = new string[] { "\"", "\\", "/", ";", "'" };
            foreach (string c in charsToRemove)
                temp = temp.Replace(c, string.Empty);

            return temp;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var image = pictureBox1.Image;
            pictureBox1.Image = _graphVisualization.Draw(pictureBox1.Width, pictureBox1.Height);
            if (image != null)
                image.Dispose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StopCapture();
        }

        private string ShortenName(string name)
        {
            int maxLength = 40;
            if (name.Length > maxLength)
                return name.Substring(0, maxLength) + "...";
            return name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartCapture(savePath + "\\" + strToFile(lastArtistName + " - " + lastTrackName + ".wav"));
            BT_Record.Enabled = false;
            BT_Stop.Enabled = true;
            BT_RecordOnPlay.Enabled = false;
            recordOnPlay = true;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            convertToMP3();
        }

        private void convertToMP3 ()
        {
            foreach (string file in Directory.GetFiles(savePath + "\\", "*.wav"))
            {
                try
                {
                    FileStream stream = File.OpenRead(file);

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = libavPath + "avconv";
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    startInfo.Arguments = "-i \"" + file + "\" -b 256k \"" + Application.StartupPath + "\\" + Path.GetFileNameWithoutExtension(file) + ".mp3\"";
                    stream.Close();
                    Process p = Process.Start(startInfo);
                    p.WaitForExit();

                    File.Delete(file);

                }
                catch { };
            }
        }

        private string toTimeString(float position, float length)
        {
            if (position < length)
                return TimeSpan.FromSeconds(position).ToString(@"mm\:ss") + "/" + TimeSpan.FromSeconds(length).ToString(@"mm\:ss");
            else
                return TimeSpan.FromSeconds(length).ToString(@"mm\:ss") + "/" + TimeSpan.FromSeconds(length).ToString(@"mm\:ss");
        }

        private void renderTransparent(Control c)
        {
            var pos = this.PointToScreen(c.Location);
            pos = pictureBox1.PointToClient(pos);
            c.Parent = pictureBox1;
            c.Location = pos;
        }

        private void BT_Settings_Click(object sender, EventArgs e)
        {
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                savePath = settingsForm.savePath;
                BT_RecordOnPlay.Enabled = true;
                BT_Record.Enabled = true;
            }
        }
    }
}
