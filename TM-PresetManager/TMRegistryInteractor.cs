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
        const string joysticksOEMPath =
            "System\\CurrentControlSet\\Control\\MediaProperties\\PrivateProperties\\Joystick\\OEM";
        const string joystickSettingsPath =
            "OEM\\JoystickSettings";
        string _deviceKeyName;
        string _settingsFullPath;
        RegistryKey _device;
        MainForm.FFBPreset preset;
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
    }
}
