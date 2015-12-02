using System;
using System.Collections.Generic;
using System.IO;

using Gilgame.SEWorkbench.Services;

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace Gilgame.SEWorkbench
{
    public class Program
    {
        public static List<Interop.AssemblyObject> Classes = new List<Interop.AssemblyObject>();

        [STAThread]
        public static void Main(string[] args)
        {
            string path = GetSandboxPath();
            if (!SandboxCopied(path))
            {
                bool result = CopySandbox();
                if(result)
                {
                    System.Windows.Forms.Application.Restart();
                }
                return;
            }

            Views.SplashScreenView splash = new Views.SplashScreenView();
            splash.Show();

            LoadClasses();
            Interop.Blueprint.RunInit();
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

        private static bool SandboxCopied(string sepath)
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
                    File.Copy(Path.Combine(sepath, assembly), Path.Combine(saveto, assembly), true);
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

        private static string GetSandboxPath()
        {
            string sepath = Services.Registry.GetValue(Microsoft.Win32.RegistryHive.LocalMachine, Registry.K_SEROOT, Registry.V_SELOC).ToString();
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
            // make a call to get serializers loaded so blueprints load faster

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
