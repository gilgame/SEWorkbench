using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;

using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench
{
    public class Program
    {
        public static bool IsAdmin
        {
            get
            {
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Process parent = null;
            if (args.Length > 1 && args[0].ToLower() == "--pid")
            {
                parent = Process.GetProcessById(Configuration.Convert.ToInteger(args[1]));
            }

            if (parent != null)
            {
                parent.Kill();
            }

            string path = Configuration.Program.SEPath;
            if (!SandboxIsCopied(path))
            {
                if (!IsAdmin)
                {
                    Restart(true);
                }

                if (!ValidPath(path))
                {
                    path = GetSandboxPath();
                }

                bool success = CopySandbox(path);
                if (success)
                {
                    Configuration.Program.SEPath = path;

                    Restart();
                }
            }

            Views.SplashScreenView splash = new Views.SplashScreenView();
            splash.Show();

            Interop.SpaceEngineers.Initialize();
            Interop.Decompiler.LoadClasses();

            splash.Close();

            StartApp();
        }

        private static void StartApp()
        {
            Gilgame.SEWorkbench.App app = new Gilgame.SEWorkbench.App();
            app.InitializeComponent();
            app.Run();
        }

        private static void Restart(bool elevate = false)
        {
            ProcessStartInfo info = new ProcessStartInfo()
            {
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                UseShellExecute = true,
                Arguments = "--pid " + Process.GetCurrentProcess().Id.ToString()
            };
            if (elevate)
            {
                info.Verb = "runas";
            }

            try
            {
                Process.Start(info);
            }
            catch (Exception ex)
            {
                MessageBox.ShowError("Failed to start the program", ex);
            }
        }

        private static void Elevate()
        {
            ProcessStartInfo info = new ProcessStartInfo()
            {
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                UseShellExecute = true,
                Verb = "runas",
                Arguments = "--pid " + Process.GetCurrentProcess().Id.ToString()
            };

            try
            {
                Process.Start(info);
            }
            catch (Exception ex)
            {
                MessageBox.ShowError("Failed to start the program with elevated privileges", ex);
            }
        }

        private static bool SandboxIsCopied(string sepath)
        {
            string local = Directory.GetCurrentDirectory();
            foreach (string assembly in GetDependenies())
            {
                string file = Path.Combine(local, assembly);
                string sefile = Path.Combine(sepath, assembly);
                if (File.Exists(file))
                {
                    FileInfo current = new FileInfo(file);
                    FileInfo official = new FileInfo(sefile);

                    if (official.LastWriteTime > current.LastWriteTime)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CopySandbox(string path)
        {
            string destination = Directory.GetCurrentDirectory();
            string source = path;

            try
            {
                foreach (string assembly in GetDependenies())
                {
                    CopyFile(Path.Combine(source, assembly), Path.Combine(destination, assembly));
                }

                return true;
            }
            catch (Exception ex)
            {
                // TODO log errors
                MessageBox.ShowError("Failed to copy the required libraries", ex);
                return false;
            }
        }

        private static void CopyFile(string source, string destination)
        {
            using (FileStream infile = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream outfile = new FileStream(destination, FileMode.Create))
                {
                    int buffer;
                    while ((buffer = infile.ReadByte()) != -1)
                    {
                        outfile.WriteByte((byte)buffer);
                    }
                }
            }
        }

        private static bool ValidPath(string path)
        {
            string exe = Path.Combine(path, "SpaceEngineers.exe");

            return File.Exists(exe);
        }

        private static string GetSandboxPath()
        {
            string sepath = Services.Registry.GetValue(
                Microsoft.Win32.RegistryHive.LocalMachine,
                Registry.K_SEROOT,
                Registry.V_SELOC,
                String.Empty
            ).ToString();

            if (String.IsNullOrEmpty(sepath) || !ValidPath(sepath))
            {
                sepath = UserGetPath();
                if (String.IsNullOrEmpty(sepath))
                {
                    return null;
                }
            }
            else
            {
                sepath = Path.Combine(sepath, "Bin");
            }
            return sepath;
        }

        private static string UserGetPath()
        {
            Services.MessageBox.ShowMessage("SE Workbench was unable to locate Space Engineers. You will now be prompted to locate SpaceEngineers.exe manually.");
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".exe",
                Filter = "Space Engineers Executable (SpaceEngineers.exe)|SpaceEngineers.exe",
                InitialDirectory = @"C:\Program Files (x86)\Steam\SteamApps\common\SpaceEngineers\Bin",
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result != null && result.Value == true)
            {
                string filename = dialog.FileName;
                if (!String.IsNullOrEmpty(filename))
                {
                    return Path.GetDirectoryName(filename);
                }
            }

            return String.Empty;
        }

        private static List<string> GetDependenies()
        {
            List<string> assemblies = new List<string>()
            {
                "HavokWrapper.dll",
                "InfinarioSDK.dll",
                "Sandbox.Common.dll",
                "Sandbox.Game.dll",
                "Sandbox.Graphics.dll",
                "SharpDX.Direct2D1.dll",
                "SharpDX.Direct3D11.dll",
                "SharpDX.DirectInput.dll",
                "SharpDX.dll",
                "SharpDX.DXGI.dll",
                "SharpDX.Toolkit.dll",
                "SharpDX.Toolkit.Graphics.dll",
                "SharpDX.XAudio2.dll",
                "SteamSDK.dll",
                "steam_api.dll",
                "System.Data.SQLite.dll",
                "VRage.Audio.dll",
                "VRage.dll",
                "VRage.Game.dll",
                "VRage.Game.XmlSerializers.dll",
                "VRage.Input.dll",
                "VRage.Library.dll",
                "VRage.Math.dll",
                "VRage.Native.dll",
                "SpaceEngineers.Game.dll",
                "SpaceEngineers.ObjectBuilders.dll",
                "SpaceEngineers.ObjectBuilders.XmlSerializers.dll",
                "MedievalEngineers.ObjectBuilders.dll",
                "MedievalEngineers.ObjectBuilders.XmlSerializers.dll",
            };
            return assemblies;
        }
    }
}