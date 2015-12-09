using Microsoft.Win32;
using System;

namespace Gilgame.SEWorkbench.Services
{
    public class Registry
    {
        public const string K_ROOT = @"SOFTWARE\Gilgame\SEWorkbench";
        public const string K_SEROOT = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 244850";
        public const string V_SELOC = "InstallLocation";

        public static object GetValue(RegistryHive hive, string path, string name, object value = null, bool try64 = false)
        {
            object result = value;

            RegistryKey key = GetBaseKey(hive, path, try64);
            if (key != null)
            {
                result = key.GetValue(name, value);
                if (result == null && !try64)
                {
                    result = GetValue(hive, path, name, value, true);
                }
            }
            return result;
        }

        public static void SetValue(RegistryHive hive, string path, string name, object value = null)
        {
            RegistryKey key = GetSubKey(hive);

            key = key.CreateSubKey(path);
            if (key != null)
            {
                key.SetValue(name, value);
            }
        }

        private static RegistryKey GetBaseKey(RegistryHive hive, string path, bool try64)
        {
            RegistryKey key = null;
            if (try64)
            {
                key = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
                return key.OpenSubKey(path, false);
            }
            else
            {
                if (hive == RegistryHive.LocalMachine)
                {
                    return Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);
                }
                else
                {
                    return Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path);
                }
            }
        }

        private static RegistryKey GetSubKey(RegistryHive hive)
        {
            RegistryKey key = null;
            if (hive == RegistryHive.LocalMachine)
            {
                key = Microsoft.Win32.Registry.LocalMachine;
            }
            else
            {
                key = Microsoft.Win32.Registry.CurrentUser;
            }
            return key;
        }
    }
}
