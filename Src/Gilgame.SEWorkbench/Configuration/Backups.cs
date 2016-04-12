using System;
using Gilgame.SEWorkbench.Services.IO;

namespace Gilgame.SEWorkbench.Configuration
{
    public static class Backups
    {
        private static string Root
        {
            get
            {
                return Path.Combine(Services.Registry.K_ROOT, "Backups");
            }
        }

        public static bool Enabled
        {
            get
            {
                return Convert.ToBoolean(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Enabled", true));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Enabled", Convert.ToBoolean(value));
            }
        }

        public static int Interval
        {
            get
            {
                return Convert.ToInteger(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Interval", 5));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Interval", Convert.ToInteger(value));
            }
        }
    }
}
