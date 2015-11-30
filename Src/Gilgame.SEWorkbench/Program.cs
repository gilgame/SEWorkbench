using System;
using System.IO;
using System.Reflection;

using Gilgame.SEWorkbench.Services;
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench
{
    public class Program
    {
        public static List<Interop.AssemblyObject> Classes = new List<Interop.AssemblyObject>();

        [STAThread]
        public static void Main(string[] args)
        {
            if (!SandboxCopied())
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

        private static bool SandboxCopied()
        {
            return File.Exists("Sandbox.Common.dll");
        }

        private static bool CopySandbox()
        {
            string saveto = Directory.GetCurrentDirectory();

            string sepath = Services.Registry.GetValue(Microsoft.Win32.RegistryHive.LocalMachine, Registry.K_SEROOT, Registry.V_SELOC).ToString();
            if (String.IsNullOrEmpty(sepath))
            {
                sepath = GetSEPath();
                if (String.IsNullOrEmpty(sepath))
                {
                    return false;
                }
            }
            else
            {
                sepath = Path.Combine(sepath, "Bin");
            }

            try
            {
                foreach (string assembly in GetDependencyNames())
                {
                    File.Copy(Path.Combine(sepath, assembly), Path.Combine(saveto, assembly));
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

        private static string GetSEPath()
        {
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
            
            return null;
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
