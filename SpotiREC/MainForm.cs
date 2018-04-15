using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.SoundIn;
using CSCore.Streams;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Linq;

namespace SpotiREC
{
    public partial class Main : Form
    {
        #region Variables
        private SettingsForm settingsForm;

        private string spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
        private string jsonPath = Application.StartupPath + @"\Newtonsoft.Json.dll";
        private string cscorePath = Application.StartupPath + @"\CSCore.dll";
        private string hostsPath = Environment.GetEnvironmentVariable("WINDIR") + @"\System32\drivers\etc\hosts";

        private string zipPath = Application.StartupPath + @"\avconv.zip";
        private string libavPath = Application.StartupPath + @"\avconv";
        public static string logPath = Application.StartupPath + @"\SpotiREC_log.txt";
        private string mp3ID3Catalog = Application.StartupPath + @"\Spotic_ID3_catalog.txt";
        private string savePath = Application.StartupPath;

        [DllImport("shell32.dll")]
        public static extern bool IsUserAnAdmin();

        private string[] adHosts = { "pubads.g.doubleclick.net", "securepubads.g.doubleclick.net", "www.googletagservices.com", "gads.pubmatic.com", "ads.pubmatic.com", "spclient.wg.spotify.com" };

        private string lastArtistName = "";
        private string lastTrackName = "";
        private string lastAlbumName = "";

        private int exitTolerance = 0;

        // Main timer
        System.Timers.Timer timer;

        bool recordOnPlay = false;
        bool recordWaiting = false;
        bool isTimerDone = true;
        bool isDrawingDone = true;
        bool convertToMp3OnNew = true; // To modify to a check box on the settings form

        // CSCore Variables
        private WasapiCapture _soundIn;
        private IWriteable _writer;
        private readonly GraphVisualization _graphVisualization = new GraphVisualization();
        private IWaveSource _finalSource;
        #endregion

        #region GUI 
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
            // Start Spotify and give EZBlocker higher priority
            try
            {
                if (File.Exists(spotifyPath) && Process.GetProcessesByName("spotify").Length < 1)
                    Process.Start(spotifyPath);
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; // Windows throttles down when minimized to task tray, so make sure EZBlocker runs smoothly

                if (!IsUserAnAdmin())
                {
                    MessageBox.Show("Removing advertising in Spotify requires Administrator privileges.\n\nPlease reopen SpotiREC with \"Run as Administrator\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!File.Exists(hostsPath))
                    File.Create(hostsPath).Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            try
            {
                // Always clear hosts
                string[] text = File.ReadAllLines(hostsPath);
                text = text.Where(line => !adHosts.Contains(line.Replace("0.0.0.0 ", "")) && line.Length > 0 && !line.Contains("open.spotify.com")).ToArray();
                File.WriteAllLines(hostsPath, text);
                    
                using (StreamWriter sw = File.AppendText(hostsPath))
                {
                    sw.WriteLine();
                    foreach (string host in adHosts)
                        sw.WriteLine("0.0.0.0 " + host);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Error trying to remove ads in Spotify", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Dependancies unzip
            try
            {
                if (!Directory.Exists(libavPath) || Directory.GetFiles(libavPath).Length != 33)
                {
                    if (File.Exists(zipPath))
                        File.Delete(zipPath);
                    File.WriteAllBytes(zipPath, Properties.Resources.avconv);

                    if (Directory.Exists(libavPath))
                        Directory.Delete(libavPath, true);
                    ZipFile.ExtractToDirectory(zipPath, libavPath);
                    File.Delete(zipPath);
                }

                if (!File.Exists(jsonPath))
                    File.WriteAllBytes(jsonPath, Properties.Resources.Newtonsoft_Json);

                if (!File.Exists(cscorePath))
                    File.WriteAllBytes(cscorePath, Properties.Resources.CSCore);

                /* Don't remove the mp3 catalog
                if (File.Exists(mp3ID3Catalog))
                    File.Delete(mp3ID3Catalog);*/
            }
            catch (Exception)
            {
                MessageBox.Show("Error trying to write SpotiREC dependancies.\nClosing application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            // .NET Framework check
            if (!hasNet45())
            {
                if (MessageBox.Show("You do not have .NET Framework 4.5. Download now?", "SpotiREC Error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    Process.Start("https://www.microsoft.com/en-us/download/details.aspx?id=30653");
                else
                    MessageBox.Show("SpotiREC may not function properly without .NET Framework 4.5 or above.");
            }

            ToolTip tt1 = new ToolTip();
            tt1.SetToolTip(BT_RecordOnPlay, "Smart Spotify audio recording");
            tt1.SetToolTip(BT_Record, "Record sound from selected device");
            tt1.SetToolTip(BT_Convert, "Convert all files from the selected directory from WAV to MP3");
            tt1.SetToolTip(BT_Stop, "Stop the recording");

            MessageBox.Show("The SpotiREC application is still experimental and may be updated very often during the testing process.\n\n" +
                "To test the functionnalities :\n" +
                "1. Select the device you want to record and the save path by clicking the gear icon in the upper right corner\n" +
                "2. Click the play button for simple record or try the smart recording (with artist & track recognition and auto stop/record function when the track changes) by clicking the lower left button.\n\n" +
                "Open the saving directory to see the live result.\n\n" /*+
                "For FAQ, tutorials and more, please check my website : www.francois_delebecq.com\n" +
                "Comments are more than appreciated ;)"*/, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += MainTimer_Tick;
            timer.Start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "WAV (*.wav)|*.wav",
                Title = "Save",
                FileName = "New Record"
            };
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                StartCapture(sfd.FileName);
                BT_Record.Enabled = false;
                BT_RecordOnPlay.Enabled = false;
                BT_Stop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCapture();

            BT_Stop.Enabled = false;
            BT_Record.Enabled = true;
            BT_RecordOnPlay.Enabled = true;
            recordOnPlay = false;
            recordWaiting = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StopCapture();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            convertToMP3();
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

        private void BT_RecordOnPlay_Click(object sender, EventArgs e)
        {
            WebHelperResult whr = WebHelperHook.GetStatus();
            if (!whr.isPlaying)
                recordWaiting = true;
            else
            {
                StartCapture(savePath + "\\" + strToFile(lastArtistName + " - " + lastTrackName + ".wav"));
                File.AppendAllText(mp3ID3Catalog, lastArtistName + " - " + lastTrackName + ".wav" + "¤" + lastTrackName + "¤" + lastArtistName + "¤" + lastAlbumName + "\n");
            }

            BT_Record.Enabled = false;
            BT_RecordOnPlay.Enabled = false;
            BT_Stop.Enabled = true;
            recordOnPlay = true;
        }
        #endregion

        #region Timer Tick (Logic)
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            // Avoid simultaneous calls 
            if (!isTimerDone)
                return;
            isTimerDone = false;

            try
            {
                if (Process.GetProcessesByName("spotify").Length < 1)
                {
                    if (exitTolerance > 10)
                    {
                        File.AppendAllText(logPath, "Spotify process not found\r\n");
                        MessageBox.Show("Spotify hasn't started or was manually closed, the application will exit.", "Closing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    if (LB_Status.InvokeRequired)
                        LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = "Spotify not started"; }));
                    
                    exitTolerance += 1;
                    isTimerDone = true;
                    return;
                }
                else
                {
                    exitTolerance = 0;
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
                        LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = "SpotifyWebHelper nor started"; }));
                    File.AppendAllText(logPath, "Not running.\r\n");
                    timer.Interval = 5000;
                }
                else if (!whr.isPlaying)
                {
                    if (recordOnPlay && !recordWaiting)
                    {
                        StopCapture();
                        recordOnPlay = false;
                        recordWaiting = false;
                    }

                    if (LB_Status.InvokeRequired)
                        LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = "Spotify paused"; }));
                    lastArtistName = "";
                    lastTrackName = "";
                }
                else // Song is playing
                {
                    if ((whr.length - whr.position < 2) || (!whr.isPlaying && recordOnPlay)) timer.Interval = 300;
                    else if (timer.Interval != 1000) timer.Interval = 1000;

                    if (lastArtistName != whr.artistName || lastTrackName != whr.trackName)
                    {                        
                        if (recordOnPlay)
                        {
                            if (!recordWaiting)
                                StopCapture();   
                                                     
                            StartCapture(savePath + "\\" + strToFile(whr.artistName + " - " + whr.trackName + ".wav"));
                            recordWaiting = false;

                            Task.Run(() => convertToMP3(true));
                            File.AppendAllText(mp3ID3Catalog, strToFile(whr.artistName + " - " + whr.trackName + ".wav") + "¤" + whr.trackName + "¤" + whr.artistName + "¤" + whr.albumName + "\n");
                        }

                        // Delegate refresh on track change
                        if (LB_Status.InvokeRequired)
                            LB_Status.Invoke(new MethodInvoker(delegate { LB_Status.Text = ShortenName("> " + whr.artistName + " - " + whr.trackName); }));
                        if (PlayingProgress.InvokeRequired)
                            PlayingProgress.Invoke(new MethodInvoker(delegate { PlayingProgress.Maximum = whr.length; }));
                        lastArtistName = whr.artistName;
                        lastTrackName = whr.trackName;
                        lastAlbumName = whr.albumName;
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

            isTimerDone = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var image = pictureBox1.Image;
            pictureBox1.Image = _graphVisualization.Draw(pictureBox1.Width, pictureBox1.Height);
            if (image != null)
                image.Dispose();
        }
        #endregion

        #region Recording functions
        private void StartCapture(string fileName)
        {
            if (settingsForm.SelectedDevice == null)
                return;

            _soundIn = new WasapiLoopbackCapture();
            _soundIn.Device = settingsForm.SelectedDevice;
            _soundIn.Initialize();

            // Check file name availability, if not available add "_" while the name exists
            string finalName = Path.ChangeExtension(fileName, null);
            while (File.Exists(finalName + ".wav"))
                finalName += "_";
            finalName += ".wav";

            var soundInSource = new SoundInSource(_soundIn);
            var singleBlockNotificationStream = new SingleBlockNotificationStream(soundInSource.ToSampleSource());
            _finalSource = singleBlockNotificationStream.ToWaveSource();
            _writer = new WaveWriter(finalName, _finalSource.WaveFormat);

            byte[] buffer = new byte[_finalSource.WaveFormat.BytesPerSecond];
            soundInSource.DataAvailable += (s, e) =>
            {
                int read = _finalSource.Read(buffer, 0, buffer.Length);
                //while ((read = _finalSource.Read(buffer, 0, buffer.Length)) > 0) Causes stops in the music recording (very bad idea)
                _writer.Write(buffer, 0, read);
            };

            singleBlockNotificationStream.SingleBlockRead += SingleBlockNotificationStreamOnSingleBlockRead;

            _soundIn.Start();
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
            }
        }

        private void SingleBlockNotificationStreamOnSingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            _graphVisualization.AddSamples(e.Left, e.Right);
        }
        #endregion

        #region MP3 Conversion
        private void convertToMP3 (bool silent = false)
        {
            string[] files = Directory.GetFiles(savePath + "\\", "*.wav");

            if (files.Length == 0)
            {
                if (!silent)
                    MessageBox.Show("There are no WAV files to convert in the " + savePath + " folder", "No file to convert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // To be replaced by a Checkbox in the settings form
            bool addId3Tags = true;
            foreach (string file in files)
            {
                try
                {
                    if (!silent)
                        LB_Status.Text = "Converting files to MP3";

                    FileStream stream = File.OpenRead(file);

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = libavPath + @"\avconv";
                    startInfo.CreateNoWindow = silent ? true : false;
                    startInfo.UseShellExecute = false;
                    startInfo.Arguments = "-i \"" + file + "\" -y -b 256k ";
                    startInfo.Arguments += addId3Tags ? metadataArguments(file) : String.Empty;
                    startInfo.Arguments += " \"" + savePath + "\\" + Path.GetFileNameWithoutExtension(file) + ".mp3\"";
                    stream.Close();
                    Process p = Process.Start(startInfo);
                    p.WaitForExit();

                    File.Delete(file);
                    Debug.WriteLine(startInfo.Arguments);

                }
                catch {}
            }
            if (silent) return;

            string[] wavFiles = Directory.GetFiles(savePath + "\\", "*.wav");
            string text = files.Length + " files processed\n" +
                (files.Length - wavFiles.Length) + " files converted\n";
            text += wavFiles.Length > 0 ? wavFiles.Length + " files were not converted (they may be in use by the recording process or another processus." : String.Empty;
 
            MessageBox.Show(text, "End of conversion process", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LB_Status.Text = ShortenName("> " + lastArtistName+ " - " + lastTrackName);
        }
        #endregion

        #region Helpers
        private string metadataArguments(string fileName)
        {
            if (!File.Exists(mp3ID3Catalog))
                return String.Empty;

            string line;
            StreamReader file = new StreamReader(mp3ID3Catalog);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Split('¤')[0] == Path.GetFileName(fileName))
                {
                    file.Close();
                    line = escape(line);
                    return " -metadata title=\"" + line.Split('¤')[1] + "\" -metadata artist=\"" + line.Split('¤')[2] + "\" -metadata album=\"" + line.Split('¤')[3] + "\" ";
                }
            }

            file.Close();
            return "";
        }

        private string strToFile(string fileName)
        {
            string temp = fileName;
            string[] charsToRemove = new string[] { "\"", "\\", "/", ";", ":" }; // 18_09_2016 Removed ' char from list 17_10_2016 Added :
            foreach (string c in charsToRemove)
                temp = temp.Replace(c, string.Empty);

            return temp;
        }

        private string ShortenName(string name)
        {
            int maxLength = 40;
            if (name.Length > maxLength)
                return name.Substring(0, maxLength) + "...";
            return name;
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

        private string escape(string text)
        {
            //"[]=;,"
            return text
                .Replace("[", "(")
                .Replace("]", ")")
                .Replace("=", "")
                .Replace(";", "")
                .Replace(",", " ");
        }

        private static bool hasNet45()
        {
            try
            {
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
                {
                    int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                    if (releaseKey >= 378389) return true;
                }
            }
            catch (Exception) { }
            return false;
        }

        #endregion
    }
}
