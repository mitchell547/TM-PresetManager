using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TM_PresetManager
{
    class TMRegistryInteractor
    {
        public MainForm.FFBPreset preset { get; private set; }
        public string TMDeviceName { get; private set; }

        private const string joysticksOEMPath =
            "System\\CurrentControlSet\\Control\\MediaProperties\\PrivateProperties\\Joystick\\OEM";
        private const string joystickSettingsPath =
            "OEM\\JoystickSettings";
        private string _deviceKeyName;
        private string _settingsFullPath;
        private RegistryKey _device;
        private string HIDDeviceID;

        public TMRegistryInteractor()
        {
            initSettingsFromRegistry();
        }

        public void SetRegistryValuesWithPreset(MainForm.FFBPreset preset)
        {
            Registry.SetValue(_settingsFullPath, "DefaultWheelAngle", convertIntToLittleEndianBin((int)preset.wheelAngle));
            Registry.SetValue(_settingsFullPath, "OverallGain", convertIntToLittleEndianBin((int)preset.overallGain * 100));
            Registry.SetValue(_settingsFullPath, "ConstantForceGain", convertIntToLittleEndianBin((int)preset.constantForceGain * 100));
            Registry.SetValue(_settingsFullPath, "PeriodicGain", convertIntToLittleEndianBin((int)preset.periodicGain * 100));
            Registry.SetValue(_settingsFullPath, "DamperGain", convertIntToLittleEndianBin((int)preset.damperGain * 100));
            Registry.SetValue(_settingsFullPath, "SpringGain", convertIntToLittleEndianBin((int)preset.springGain * 100));
        }
        private int convertRegValueToInt(object value)
        {
            // Register values have little-endian byte order
            // So, reverse them and convert to int
            if (value == null)
                return -1;
            Byte[] data = (Byte[])value;
            data.Reverse();
            return BitConverter.ToInt32(data, 0);
        }

        private Byte[] convertIntToLittleEndianBin(int value)
        {
            Byte[] data = BitConverter.GetBytes(value);
            data.Reverse();
            return data;
        }
        private void initSettingsFromRegistry()
        {
            RegistryKey OEM = Registry.CurrentUser.OpenSubKey(joysticksOEMPath);
            if (OEM == null)
            {
                throw new Exception("Joysticks registry not found");
            }
            List<RegistryKey> foundDevices = new List<RegistryKey>();
            {
                string[] subKeys = OEM.GetSubKeyNames();
                foreach (string k in subKeys)
                {
                    string devicePath = joysticksOEMPath + "\\" + k + "\\" + joystickSettingsPath;
                    RegistryKey deviceSettingsKey = Registry.CurrentUser.OpenSubKey(devicePath);
                    if (deviceSettingsKey != null)
                    {
                        RegistryKey deviceKey = Registry.CurrentUser.OpenSubKey(joysticksOEMPath + "\\" + k);
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
                HIDDeviceID = System.IO.Path.GetFileName(_device.Name);
                string deviceName = (string)_device.GetValue("OEMName");
                RegistryKey settings = getSettingsKeyFromJoystick(_device);
                _settingsFullPath = settings.Name;

                int wheelAngle = convertRegValueToInt(settings.GetValue("DefaultWheelAngle"));
                int overallGain = convertRegValueToInt(settings.GetValue("OverallGain"));
                int constantForce = convertRegValueToInt(settings.GetValue("ConstantForceGain"));
                int periodicGain = convertRegValueToInt(settings.GetValue("PeriodicGain"));
                int damper = convertRegValueToInt(settings.GetValue("DamperGain"));
                int spring = convertRegValueToInt(settings.GetValue("SpringGain"));
                // Gain values range [0; 100000] (max = 100%)
                // Wheel angle range [0; 1080]

                TMDeviceName = deviceName;
                preset = new MainForm.FFBPreset();
                preset.wheelAngle           = wheelAngle >= 0 ?     wheelAngle          : preset.wheelAngle;
                preset.overallGain          = overallGain >= 0 ?    overallGain / 100   : preset.overallGain;
                preset.constantForceGain    = constantForce >= 0 ?  constantForce / 100 : preset.constantForceGain;
                preset.periodicGain         = periodicGain >= 0 ?   periodicGain / 100  : preset.periodicGain;
                preset.damperGain           = damper >= 0 ?         damper / 100        : preset.damperGain;
                preset.springGain           = spring >= 0 ?         spring / 100        : preset.springGain;
            }
        }

        private string extractDeviceHexIdByPrefix(string prefix)
        {
            if (HIDDeviceID.Length == 0)
                return "";
            int idx = HIDDeviceID.IndexOf(prefix);
            if (idx < 0)
                return "";
            idx += 4;
            string hexId = HIDDeviceID.Substring(idx, 4);
            return "0x" + hexId;
        }

        public string ExtractHexVendorID()
        {
            return extractDeviceHexIdByPrefix("VID_");
        }

        public string ExtractHexProductID()
        {
            return extractDeviceHexIdByPrefix("PID_");
        }

        private RegistryKey getSettingsKeyFromJoystick(RegistryKey joystickKey)
        {
            RegistryKey settings = joystickKey.OpenSubKey(joystickSettingsPath);
            string[] subKeys = settings.GetSubKeyNames();

            // TODO: check how many subkey there are

            settings = settings.OpenSubKey(subKeys[0]);
            return settings;
        }

    }
}
