using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//using HidLibrary;

namespace TM_PresetManager
{
    public partial class MainForm : Form
    {
        const string presetsFileName = "presets.xml";

        TMRegistryInteractor regInteractor;

        BindingList<FFBPreset> _presets = new BindingList<FFBPreset>();

        //HidDevice _hidDevice;

        [Serializable]
        public class FFBPreset
        {
            public string name { get; set; } = "Default";
            public int wheelAngle { get; set; } = 1080;
            public int overallGain { get; set; } = 75;
            public int constantForceGain { get; set; } = 100;
            public int periodicGain { get; set; } = 100;
            public int damperGain { get; set; } = 100;
            public int springGain { get; set; } = 100;
        };

        public MainForm()
        {
            InitializeComponent();

            presetListBox.DisplayMember = "name";
            {
                if (File.Exists(presetsFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(BindingList<FFBPreset>));
                    using (FileStream fs = new FileStream(presetsFileName, FileMode.Open))
                    {
                        _presets = (BindingList<FFBPreset>)serializer.Deserialize(fs);
                    }
                }
            }
            presetListBox.DataSource = _presets;
            presetListBox.ClearSelected();

            try
            {
                regInteractor = new TMRegistryInteractor();
                initSettingsFromRegistry();
            }
            catch (Exception exception)
            { 
                System.Windows.Forms.MessageBox.Show(
                    "Wheel cannot be found.\n" +
                    "Are you sure you are using Thrustmaster wheel and have installed driver?\n");
                this.Load += (s, e) => Close();
            }

            //{
            //    string s = regInteractor.ExtractHexVendorID();
            //    int vendorId = Convert.ToInt32(regInteractor.ExtractHexVendorID(), 16);
            //    int productId = Convert.ToInt32(regInteractor.ExtractHexProductID(), 16);

            //    // TODO: check if there are multiple devices found
            //    _hidDevice = HidDevices.Enumerate(vendorId, productId).FirstOrDefault();

            //    if (_hidDevice != null)
            //    {
            //        deviceStatusLbl.Text = "Conected";
            //        _hidDevice.OpenDevice();

            //        HidDeviceData InData;
            //        string Text;

            //        InData = _hidDevice.Read();
            //        Text = System.Text.ASCIIEncoding.ASCII.GetString(InData.Data);

            //        Console.WriteLine(Text);
            //    }
            //    else
            //        deviceStatusLbl.Text = "Not Conected";
            //}

            wheelAngleValue.ValueChanged += new System.EventHandler(markIfSaveNeeded);
            strengthValue.ValueChanged += new System.EventHandler(markIfSaveNeeded);
            constantGainValue.ValueChanged += new System.EventHandler(markIfSaveNeeded);
            periodicGainValue.ValueChanged += new System.EventHandler(markIfSaveNeeded);
            damperGainValue.ValueChanged += new System.EventHandler(markIfSaveNeeded);
            springGainValue.ValueChanged += new System.EventHandler(markIfSaveNeeded);

            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            updateNotifyMenu();
        }



        private FFBPreset getSelectedPreset()
        {
            return (FFBPreset)presetListBox.SelectedItem;
        }

        private void initSettingsFromRegistry()
        {            
            WheelNameLbl.Text = regInteractor.TMDeviceName;
            FFBPreset preset = regInteractor.preset;
            wheelAngleValue.Value = preset.wheelAngle;
            strengthValue.Value = preset.overallGain;
            constantGainValue.Value = preset.constantForceGain;
            periodicGainValue.Value = preset.periodicGain;
            damperGainValue.Value = preset.damperGain;
            springGainValue.Value = preset.springGain;

        }

        private void restoreWindow()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void btnOpenOnClick(object sender, System.EventArgs e)
        {
            restoreWindow();
        }

        private void btnExitOnClick(object sender, System.EventArgs e)
        {
            Close();
        }

        private void notifyPresetBtnOnClick(object sender, System.EventArgs e, int presetIndex)
        {
            presetListBox.SelectedIndex = presetIndex;
            useSelectedPreset();
        }

        private void updateNotifyMenu()
        {
            notifyIcon.ContextMenuStrip.Items.Clear();
            
            ToolStripButton btnOpen = new ToolStripButton();
            btnOpen.Text = "Open";
            btnOpen.Click += new System.EventHandler(btnOpenOnClick);

            ToolStripButton btnExit = new ToolStripButton();
            btnExit.Text = "Exit";
            btnExit.Click += new System.EventHandler(btnExitOnClick);

            notifyIcon.ContextMenuStrip.Items.Add(btnOpen);
            notifyIcon.ContextMenuStrip.Items.Add(btnExit);
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            //FFBPreset curPreset = getSelectedPreset();

            for (int i = 0; i < _presets.Count; ++i)
            {
                FFBPreset preset = _presets[i];
                ToolStripButton presetBtn = new ToolStripButton();
                presetBtn.Text = preset.name;
                int id = i;
                presetBtn.Click += (sender, EventArgs) => { notifyPresetBtnOnClick(sender, EventArgs, id); };
                notifyIcon.ContextMenuStrip.Items.Add(presetBtn);
            }
        }

        private void createNewBtn_Click(object sender, EventArgs e)
        {
            PresetNameDialog dialog = new PresetNameDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult != DialogResult.OK)
                return;

            FFBPreset preset = new FFBPreset();
            preset.name = dialog.presetName;
            preset.wheelAngle = (int)wheelAngleValue.Value;
            preset.overallGain = (int)strengthValue.Value;
            preset.constantForceGain = (int)constantGainValue.Value;
            preset.periodicGain = (int)periodicGainValue.Value;
            preset.damperGain = (int)damperGainValue.Value;
            preset.springGain = (int)springGainValue.Value;
            _presets.Add(preset);

            presetListBox.SelectedIndex = presetListBox.Items.IndexOf(preset);
            useSelectedPreset();
            saveAllPresets();
            updateNotifyMenu();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            //int id = (int)presetListBox.SelectedValue;
            FFBPreset preset = getSelectedPreset();
            _presets.Remove(preset);
            saveAllPresets();
            useSelectedPreset();
            updateNotifyMenu();
        }

        private void presetListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            useSelectedPreset();
        }

        private void updatePresetBtn_Click(object sender, EventArgs e)
        {
            FFBPreset preset = getSelectedPreset();
            preset.wheelAngle = (int)wheelAngleValue.Value;
            preset.overallGain = (int)strengthValue.Value;
            preset.constantForceGain = (int)constantGainValue.Value;
            preset.periodicGain = (int)periodicGainValue.Value;
            preset.damperGain = (int)damperGainValue.Value;
            preset.springGain = (int)springGainValue.Value;

            useSelectedPreset();
        }

        private void markIfSaveNeeded(object sender, System.EventArgs e)
        {
            FFBPreset preset = getSelectedPreset();
            if (preset == null || preset.name != presetLbl.Text)
                return;
            if (updatePresetBtn.Text.EndsWith("*"))
                return;
            updatePresetBtn.Text = updatePresetBtn.Text + "*";
        }

        private void resetUpdatePresetBtnText()
        {
            updatePresetBtn.Text = "Update Preset";
        }

        private void saveAllPresets()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BindingList<FFBPreset>));
            using (FileStream fs = new FileStream(presetsFileName, FileMode.Create))
            {
                serializer.Serialize(fs, _presets);
            }
        }

        private void fillPresetControls(FFBPreset preset)
        {
            presetLbl.Text = preset.name;
            wheelAngleValue.Value = preset.wheelAngle;
            strengthValue.Value = preset.overallGain;
            constantGainValue.Value = preset.constantForceGain;
            periodicGainValue.Value = preset.periodicGain;
            damperGainValue.Value = preset.damperGain;
            springGainValue.Value = preset.springGain;
        }
       
        private void useSelectedPreset()
        {
            FFBPreset preset = getSelectedPreset();
            fillPresetControls(preset);
            //setRegistryValuesWithPreset(preset);
            regInteractor.SetRegistryValuesWithPreset(preset);
            resetUpdatePresetBtnText();
            saveAllPresets();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            restoreWindow();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.BalloonTipText = "I'm there now";
                //notifyIcon.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (_hidDevice == null || !_hidDevice.IsConnected)
            //    return;
            //_hidDevice.CloseDevice();
            //_hidDevice.Dispose();
            //_hidDevice = (HidDevice)null;
        }

        //private void readHIDBtn_Click(object sender, EventArgs e)
        //{
        //    if (_hidDevice != null)
        //    {

        //        HidDeviceData InData;
        //        string Text;

        //        InData = _hidDevice.Read();
        //        Text = System.Text.ASCIIEncoding.ASCII.GetString(InData.Data);

        //        Console.WriteLine(Text);
        //    }
        //}
    }
}
