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
        public static string logPath = Application.StartupPath + @"\SpotiREC_log.txt";
        public string libavPath = @"C:\youdl\";
        private string lastArtistName = "";
        private string lastTrackName = "";
        System.Timers.Timer timer;
        bool recordOnPlay = false;

        // CSCore Variables
        private const CaptureMode CaptureMode = SpotiREC.CaptureMode.LoopbackCapture;

        private MMDevice _selectedDevice;
        private WasapiCapture _soundIn;
        private IWriteable _writer;
        private readonly GraphVisualization _graphVisualization = new GraphVisualization();
        private IWaveSource _finalSource;

        public MMDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                if (value != null)
                {
                    btnStart.Enabled = true;
                    BT_RecordOnPlay.Enabled = true;
                }
            }
        }

        public Main()
        {
            InitializeComponent();
            renderTransparent(LB_Status);
            renderTransparent(LB_Position);
            renderTransparent(BT_RecordOnPlay);
            renderTransparent(BT_Settings);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += MainTimer_Tick;
            timer.Start();

            RefreshDevices();
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
                    if (whr.length - whr.position < 5) timer.Interval = 300;
                    else if (MainTimer.Interval != 1000) timer.Interval = 1000;

                    if (lastArtistName != whr.artistName || lastTrackName != whr.trackName)
                    {
                        // Delegate refresh on track change
                        if (recordOnPlay)
                        {
                            StopCapture();
                            StartCapture(Application.StartupPath + "\\" + strToFile(whr.artistName + " - " + whr.trackName + ".wav"));
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

        private void RefreshDevices()
        {
            deviceList.Items.Clear();

            using (var deviceEnumerator = new MMDeviceEnumerator())
            using (var deviceCollection = deviceEnumerator.EnumAudioEndpoints(
                CaptureMode == CaptureMode.Capture ? DataFlow.Capture : DataFlow.Render, DeviceState.Active))
            {
                foreach (var device in deviceCollection)
                {
                    var deviceFormat = WaveFormatFromBlob(device.PropertyStore[
                        new PropertyKey(new Guid(0xf19f064d, 0x82c, 0x4e27, 0xbc, 0x73, 0x68, 0x82, 0xa1, 0xbb, 0x8e, 0x4c), 0)].BlobValue);

                    var item = new ListViewItem(device.FriendlyName) { Tag = device };
                    item.SubItems.Add(deviceFormat.Channels.ToString(CultureInfo.InvariantCulture));

                    deviceList.Items.Add(item);
                }
            }
        }

        private void StartCapture(string fileName)
        {
            if (SelectedDevice == null)
                return;

            if (CaptureMode == CaptureMode.Capture)
                _soundIn = new WasapiCapture();
            else
                _soundIn = new WasapiLoopbackCapture();

            _soundIn.Device = SelectedDevice;         
            _soundIn.Initialize();

            var soundInSource = new SoundInSource(_soundIn);
            var singleBlockNotificationStream = new SingleBlockNotificationStream(soundInSource.ToSampleSource());
            _finalSource = singleBlockNotificationStream.ToWaveSource();
            _writer = new WaveWriter(fileName, _finalSource.WaveFormat);
            // Simplify the file name

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

        private static WaveFormat WaveFormatFromBlob(Blob blob)
        {
            if (blob.Length == 40)
                return (WaveFormat)Marshal.PtrToStructure(blob.Data, typeof(WaveFormatExtensible));
            return (WaveFormat)Marshal.PtrToStructure(blob.Data, typeof(WaveFormat));
        }

        private void btnRefreshDevices_Click(object sender, EventArgs e)
        {
            RefreshDevices();
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
                btnStart.Enabled = false;
                btnStop.Enabled = true;
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
                    btnStop.Enabled = false;
                    btnStart.Enabled = true;
                }
            }
        }

        private void deviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceList.SelectedItems.Count > 0)
            {
                SelectedDevice = (MMDevice)deviceList.SelectedItems[0].Tag;
            }
            else
            {
                SelectedDevice = null;
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
            StartCapture(Application.StartupPath + "\\" + strToFile(lastArtistName + " - " + lastTrackName + ".wav"));
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            BT_RecordOnPlay.Enabled = false;
            recordOnPlay = true;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            convertToMP3();
        }

        private void convertToMP3 ()
        {
            foreach (string file in Directory.GetFiles(Application.StartupPath + "\\", "*.wav"))
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

        private string toTimeString(float seconds, float length)
        {
            if (seconds < length)
                return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss") + "/" + TimeSpan.FromSeconds(length).ToString(@"mm\:ss");
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

    }

    public enum CaptureMode
    {
        Capture,
        LoopbackCapture
    }
}
