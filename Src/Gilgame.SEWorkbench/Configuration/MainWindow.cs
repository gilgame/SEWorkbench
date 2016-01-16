using System;
using Gilgame.SEWorkbench.Services.IO;

namespace Gilgame.SEWorkbench.Configuration
{
    public static class MainWindow
    {
        private static string Root
        {
            get
            {
                return Path.Combine(Services.Registry.K_ROOT, "MainWindow");
            }
        }

        public static double Width
        {
            get
            {
                return Convert.ToDouble(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Width", "1024"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Width", Convert.ToString(value));
            }
        }

        public static double Height
        {
            get
            {
                return Convert.ToDouble(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Height", "576"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Height", Convert.ToString(value));
            }
        }

        public static double Left
        {
            get
            {
                return Convert.ToDouble(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Left", "10"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Left", Convert.ToString(value));
            }
        }

        public static double Top
        {
            get
            {
                return Convert.ToDouble(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Top", "10"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "Top", Convert.ToString(value));
            }
        }

        public static System.Windows.WindowState WindowState
        {
            get
            {
                return Convert.ToWindowState(Services.Registry.GetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "WindowState", "Normal"));
            }
            set
            {
                Services.Registry.SetValue(Microsoft.Win32.RegistryHive.CurrentUser, Root, "WindowState", Convert.ToString(value));
            }
        }
    }
}
