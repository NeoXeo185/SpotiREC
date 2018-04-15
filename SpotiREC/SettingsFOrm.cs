using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSCore;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Win32;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SpotiREC
{
    public partial class SettingsForm : Form
    {
        public string savePath = Application.StartupPath;
        private const CaptureMode CaptureMode = SpotiREC.CaptureMode.LoopbackCapture;
        private MMDevice _selectedDevice;

        public MMDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                if (value != null)
                {
                    BT_OK.Enabled = true;
                }
            }
        }

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void DeviceSelectionForm_Load(object sender, EventArgs e)
        {
            RefreshDevices();
            TB_SavingPath.Text = savePath;
        }

        private void RefreshDevices()
        {
            LV_DeviceList.Items.Clear();

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

                    LV_DeviceList.Items.Add(item);
                }
            }
        }

        private static WaveFormat WaveFormatFromBlob(Blob blob)
        {
            if (blob.Length == 40)
                return (WaveFormat)Marshal.PtrToStructure(blob.Data, typeof(WaveFormatExtensible));
            return (WaveFormat)Marshal.PtrToStructure(blob.Data, typeof(WaveFormat));
        }

        private void LV_DeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_DeviceList.SelectedItems.Count > 0)
                SelectedDevice = (MMDevice)LV_DeviceList.SelectedItems[0].Tag;
            else
                SelectedDevice = null;
        }

        private void BT_SavingPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            if (browser.ShowDialog() == DialogResult.OK)
            {
                savePath = browser.SelectedPath;
                TB_SavingPath.Text = savePath;
            }
        }
    }

    public enum CaptureMode
    {
        Capture,
        LoopbackCapture
    }
}
