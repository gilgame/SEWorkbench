using System;
using Gilgame.SEWorkbench.Services.IO;

namespace Gilgame.SEWorkbench.Configuration
{
    public static class Program
    {
        private static string Root
        {
            get
            {
                return Path.Combine(Services.Registry.K_ROOT, "Program");
            }
        }

        public static string SEPath
        {
            get
            {
                return Convert.ToString(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "SEPath", String.Empty));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "SEPath", Convert.ToString(value));
            }
        }

        public static bool CheckForUpdates
        {
            get
            {
                return Convert.ToBoolean(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "CheckForUpdates", true));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "CheckForUpdates", Convert.ToBoolean(value));
            }
        }
    }
}
