using System;
using System.IO;

namespace Gilgame.SEWorkbench.Configuration
{
    public static class TextEditor
    {
        private static string Root
        {
            get
            {
                return Path.Combine(Services.Registry.K_ROOT, "TextEditor");
            }
        }

        public static string FontFamily
        {
            get
            {
                return Convert.ToString(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "FontFamily", "Consolas"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "FontFamily", value);
            }
        }

        public static double FontSize
        {
            get
            {
                return Convert.ToDouble(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "FontSize", "11"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "FontSize", Convert.ToString(value));
            }
        }

        public static bool ConvertTabsToSpaces
        {
            get
            {
                return Convert.ToBoolean(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "ConvertTabsToSpaces", "true"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "ConvertTabsToSpaces", Convert.ToString(value));
            }
        }

        public static int IndentationSize
        {
            get
            {
                return Convert.ToInteger(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "IndentationSize", "4"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "IndentationSize", Convert.ToString(value));
            }
        }
    }
}
