using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;

using Gilgame.SEWorkbench.Services;

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace Gilgame.SEWorkbench
{
    public class Program
    {
        public static List<Interop.AssemblyObject> Classes = new List<Interop.AssemblyObject>();

        public static bool IsAdmin
        {
            get
            {
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        [STAThread]
        public static void Main()
        {
            string path = GetSandboxPath();
            if (!SandboxIsCopied(path))
            {
                if (!IsAdmin)
                {
                    Elevate();
                }

                bool success = CopySandbox();
                if (success)
                {
                    System.Windows.Forms.Application.Restart();
                }
                return;
            }

            Views.SplashScreenView splash = new Views.SplashScreenView();
            splash.Show();

            LoadClasses();
            Interop.Blueprint.RunInit();
            Interop.InGameScript.Init();
            LoadSerializers();

            splash.Close();

            StartApp();
        }

        private static void StartApp()
        {
            Gilgame.SEWorkbench.App app = new Gilgame.SEWorkbench.App();
            app.InitializeComponent();
            app.Run();
        }

        private static void Elevate()
        {
            ProcessStartInfo info = new ProcessStartInfo()
            {
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                UseShellExecute = true,
                Verb = "runas"
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
            foreach(string assembly in GetDependencyNames())
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

        private static bool CopySandbox(string path = null)
        {
            string saveto = Directory.GetCurrentDirectory();
            string sepath = (path == null) ? GetSandboxPath() : path;

            try
            {
                foreach (string assembly in GetDependencyNames())
                {
                    CopyFile(Path.Combine(sepath, assembly), Path.Combine(saveto, assembly));
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

        private static string GetSandboxPath()
        {
            string sepath = Services.Registry.GetValue(
                Microsoft.Win32.RegistryHive.LocalMachine,
                Registry.K_SEROOT,
                Registry.V_SELOC,
                String.Empty
            ).ToString();

            if (String.IsNullOrEmpty(sepath))
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

        private static List<string> GetDependencyNames()
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
                "System.Data.SQLite.dll",
                "VRage.Audio.dll",
                "VRage.dll",
                "VRage.Game.dll",
                "VRage.Game.XmlSerializers.dll",
                "VRage.Input.dll",
                "VRage.Library.dll",
                "VRage.Math.dll",
                "VRage.Native.dll"
            };
            return assemblies;
        }

        private static string UserGetPath()
        {
            string path = Configuration.Program.SEPath;

            if (String.IsNullOrEmpty(path) || !File.Exists(Path.Combine(path, "Sandbox.Common.dll")))
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
                        string found = Path.GetDirectoryName(filename);

                        Configuration.Program.SEPath = found;
                        return found;
                    }
                }

                return null;
            }
            else
            {
                return path;
            }
        }

        private static void LoadSerializers()
        {
            MyObjectBuilder_Definitions loaded = null;

            try { MyObjectBuilderSerializer.DeserializeXML(String.Empty, out loaded); }
            catch { }

        }

        private static void LoadClasses()
        {
            List<string> namespaces = new List<string>()
            {
                "Sandbox.ModAPI.Ingame",
                "Sandbox.ModAPI.Interfaces",
                "VRageMath",
                "VRage.Game"
            };
            Interop.Decompiler decompiler = new Interop.Decompiler(namespaces);

            List<string> assemblies = new List<string>()
            {
                "Sandbox.Common.dll",
                "VRage.Game.dll",
                "VRage.Math.dll"
            };
            List<Interop.AssemblyObject> result = decompiler.Read(assemblies);

            Classes.AddRange(result);
        }
    }
}
