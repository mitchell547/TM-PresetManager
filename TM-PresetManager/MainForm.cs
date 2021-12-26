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
using HidLibrary;

namespace TM_PresetManager
{
    public partial class MainForm : Form
    {
        const string joysticksOEMPath =
            "System\\CurrentControlSet\\Control\\MediaProperties\\PrivateProperties\\Joystick\\OEM";
        const string joystickSettingsPath =
            "OEM\\JoystickSettings";
        string _deviceKeyName;
        string _settingsFullPath;
        RegistryKey _device;

        BindingList<FFBPreset> _presets = new BindingList<FFBPreset>();

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

        private int ConvertRegValueToInt(object value)
        {
            // Register values have little-endian byte order
            // So, reverse them and convert to int
            if (value == null)
                return 0;
            Byte[] data = (Byte[])value;
            data.Reverse();
            return BitConverter.ToInt32(data, 0);
        }

        private Byte[] ConvertIntToLittleEndianBin(int value)
        {
            Byte[] data = BitConverter.GetBytes(value);
            data.Reverse();
            return data;
        }

        public MainForm()
        {
            InitializeComponent();

            presetListBox.DisplayMember = "name";
            {
                if (File.Exists("configs.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(BindingList<FFBPreset>));
                    using (FileStream fs = new FileStream("configs.xml", FileMode.Open))
                    {
                        _presets = (BindingList<FFBPreset>)serializer.Deserialize(fs);
                    }
                }
            }
            presetListBox.DataSource = _presets;
            presetListBox.ClearSelected();
            initSettingsFromRegistry();

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
            RegistryKey OEM = Registry.CurrentUser.OpenSubKey(joysticksOEMPath);
            List<RegistryKey> foundDevices = new List<RegistryKey>();
            {
                string[] subKeys = OEM.GetSubKeyNames();
                foreach (string k in subKeys)
                {
                    string devicePath = joysticksOEMPath + "\\" + k + "\\" + joystickSettingsPath;
                    RegistryKey deviceKey = Registry.CurrentUser.OpenSubKey(devicePath);
                    if (deviceKey != null)
                    {
                        deviceKey = Registry.CurrentUser.OpenSubKey(joysticksOEMPath + "\\" + k);
                        string deviceName = (string)deviceKey.GetValue("OEMName");

                        if (deviceName.StartsWith("Thrustmaster"))
                            foundDevices.Add(deviceKey);
                    }
                }
            }

            if (foundDevices.Count == 1)
            {
                _device = foundDevices[0];
                _deviceKeyName = _device.Name;
                string deviceName = (string)_device.GetValue("OEMName");
                RegistryKey settings = GetSettingsKeyFromJoystick(_device);
                _settingsFullPath = settings.Name;

                // This setting values may not exist if they didn't ever changed from default value
                // so TODO: check value for existence, create if there is no value

                int wheelAngle = ConvertRegValueToInt(settings.GetValue("DefaultWheelAngle"));
                int overallGain = ConvertRegValueToInt(settings.GetValue("OverallGain"));
                int constantForce = ConvertRegValueToInt(settings.GetValue("ConstantForceGain"));
                int periodicGain = ConvertRegValueToInt(settings.GetValue("PeriodicGain"));
                int damper = ConvertRegValueToInt(settings.GetValue("DamperGain"));
                int spring = ConvertRegValueToInt(settings.GetValue("SpringGain"));
                // Gain values range [0; 100000] (max = 100%)
                // Wheel angle range [0; 1080]

                WheelNameLbl.Text = deviceName;
                wheelAngleValue.Value = wheelAngle;
                strengthValue.Value = overallGain / 100;
                constantGainValue.Value = constantForce / 100;
                periodicGainValue.Value = periodicGain / 100;
                damperGainValue.Value = damper / 100;
                springGainValue.Value = spring / 100;
            }
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

        private RegistryKey GetSettingsKeyFromJoystick(RegistryKey joystickKey)
        {
            RegistryKey settings = joystickKey.OpenSubKey(joystickSettingsPath);
            string[] subKeys = settings.GetSubKeyNames();

            // TODO: check how many subkey there are

            settings = settings.OpenSubKey(subKeys[0]);
            return settings;
        }

        //private void ReadValues_Click(object sender, EventArgs e)
        //{
            
        //    //const string registryPath = "HKEY_CURRENT_USER\\System\\";
        //    //RegistryKey OEM = (RegistryKey)Registry.GetValue(registryPath, "CurrentControlSet", Registry.Users);
        //    RegistryKey OEM = Registry.CurrentUser.OpenSubKey(joysticksOEMPath);
        //    List<RegistryKey> foundDevices = new List<RegistryKey>();
        //    {
        //        string[] subKeys = OEM.GetSubKeyNames();
        //        foreach (string k in subKeys)
        //        {
        //            string devicePath = joysticksOEMPath + "\\" + k + "\\" + joystickSettingsPath;
        //            RegistryKey deviceKey = Registry.CurrentUser.OpenSubKey(devicePath);
        //            if (deviceKey != null)
        //            {
        //                deviceKey = Registry.CurrentUser.OpenSubKey(joysticksOEMPath + "\\" + k);
        //                string deviceName = (string)deviceKey.GetValue("OEMName");

        //                //RegistrySecurity rs = new RegistrySecurity();
        //                //rs = deviceKey.GetAccessControl();
        //                //string user = Environment.UserDomainName + "\\" + Environment.UserName;
        //                //rs.AddAccessRule(
        //                //    new RegistryAccessRule(
        //                //        user,
        //                //        RegistryRights.WriteKey
        //                //        | RegistryRights.ReadKey
        //                //        | RegistryRights.Delete
        //                //        | RegistryRights.FullControl,
        //                //        AccessControlType.Allow));

        //                if (deviceName.StartsWith("Thrustmaster"))
        //                    foundDevices.Add(deviceKey);
        //            }
        //        }
        //    }
            
        //    if (foundDevices.Count == 1)
        //    {
        //        _device = foundDevices[0];
        //        _deviceKeyName = _device.Name;
        //        string deviceName = (string)_device.GetValue("OEMName");
        //        WheelNameLbl.Text = deviceName;
        //        RegistryKey settings = GetSettingsKeyFromJoystick(_device);
        //        _settingsFullPath = settings.Name;
        //        // This setting values may not exist if they didn't ever changed from default value
        //        // so TODO: check value for existence, create if there is no value
        //        int wheelAngle = ConvertRegValueToInt(settings.GetValue("DefaultWheelAngle"));
        //        int overallGain = ConvertRegValueToInt(settings.GetValue("OverallGain"));
        //        int constantForce = ConvertRegValueToInt(settings.GetValue("ConstantForceGain"));
        //        int periodicGain = ConvertRegValueToInt(settings.GetValue("PeriodicGain"));
        //        int damper = ConvertRegValueToInt(settings.GetValue("DamperGain"));                
        //        int spring = ConvertRegValueToInt(settings.GetValue("SpringGain"));
        //        // Gain values range [0; 100000] (max = 100%)
        //        // Wheel angle range [0; 1080]

        //        wheelAngleValue.Value = wheelAngle;
        //        strengthValue.Value = overallGain / 100;
        //        constantGainValue.Value = constantForce / 100;
        //        periodicGainValue.Value = periodicGain / 100;
        //        damperGainValue.Value = damper / 100;
        //        springGainValue.Value = spring / 100;
        //    }
        //}

        //private void WriteValues_Click(object sender, EventArgs e)
        //{
        //    if (_device == null)
        //        return;

        //    //RegistryKey settings = GetSettingsKeyFromJoystick(_device);
        //    //settings.SetValue("DamperGain", ConvertIntToLittleEndian((int)damperGainValue.Value));
        //    string settingsPath = _deviceKeyName + "\\" + joystickSettingsPath;
        //    Registry.SetValue(_settingsFullPath, "DamperGain", ConvertIntToLittleEndianBin((int)damperGainValue.Value * 100));

        //    saveAllPresets();
        //}

        
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
            using (FileStream fs = new FileStream("configs.xml", FileMode.Create))
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

        private void setRegistryValuesWithPreset(FFBPreset preset)
        {
            Registry.SetValue(_settingsFullPath, "DefaultWheelAngle", ConvertIntToLittleEndianBin((int)preset.wheelAngle));
            Registry.SetValue(_settingsFullPath, "OverallGain", ConvertIntToLittleEndianBin((int)preset.overallGain * 100));
            Registry.SetValue(_settingsFullPath, "ConstantForceGain", ConvertIntToLittleEndianBin((int)preset.constantForceGain * 100));
            Registry.SetValue(_settingsFullPath, "PeriodicGain", ConvertIntToLittleEndianBin((int)preset.periodicGain * 100));
            Registry.SetValue(_settingsFullPath, "DamperGain", ConvertIntToLittleEndianBin((int)preset.damperGain * 100));
            Registry.SetValue(_settingsFullPath, "SpringGain", ConvertIntToLittleEndianBin((int)preset.springGain * 100));
        }
       
        private void useSelectedPreset()
        {
            FFBPreset preset = getSelectedPreset();
            fillPresetControls(preset);
            setRegistryValuesWithPreset(preset);
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
    }
}
