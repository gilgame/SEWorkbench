using System;
using System.IO;
using System.Reflection;

using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (!SandboxCopied())
            {
                CopySandbox();
            }

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

        private static void CopySandbox()
        {
            string saveto = Directory.GetCurrentDirectory();

            string sepath = Services.Registry.GetValue(Microsoft.Win32.RegistryHive.LocalMachine, Registry.K_SEROOT, Registry.V_SELOC).ToString();
            if (String.IsNullOrEmpty(sepath))
            {
                sepath = GetSEPath();
                if (String.IsNullOrEmpty(sepath))
                {
                    return;
                }
            }
            else
            {
                sepath = Path.Combine(sepath, "Bin");
            }

            try
            {
                File.Copy(Path.Combine(sepath, "HavokWrapper.dll"), Path.Combine(saveto, "HavokWrapper.dll"));
                File.Copy(Path.Combine(sepath, "InfinarioSDK.dll"), Path.Combine(saveto, "InfinarioSDK.dll"));
                File.Copy(Path.Combine(sepath, "Sandbox.Common.dll"), Path.Combine(saveto, "Sandbox.Common.dll"));
                File.Copy(Path.Combine(sepath, "Sandbox.Game.dll"), Path.Combine(saveto, "Sandbox.Game.dll"));
                File.Copy(Path.Combine(sepath, "Sandbox.Graphics.dll"), Path.Combine(saveto, "Sandbox.Graphics.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.Direct2D1.dll"), Path.Combine(saveto, "SharpDX.Direct2D1.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.Direct3D11.dll"), Path.Combine(saveto, "SharpDX.Direct3D11.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.DirectInput.dll"), Path.Combine(saveto, "SharpDX.DirectInput.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.dll"), Path.Combine(saveto, "SharpDX.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.DXGI.dll"), Path.Combine(saveto, "SharpDX.DXGI.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.Toolkit.dll"), Path.Combine(saveto, "SharpDX.Toolkit.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.Toolkit.Graphics.dll"), Path.Combine(saveto, "SharpDX.Toolkit.Graphics.dll"));
                File.Copy(Path.Combine(sepath, "SharpDX.XAudio2.dll"), Path.Combine(saveto, "SharpDX.XAudio2.dll"));
                File.Copy(Path.Combine(sepath, "SteamSDK.dll"), Path.Combine(saveto, "SteamSDK.dll"));
                File.Copy(Path.Combine(sepath, "System.Data.SQLite.dll"), Path.Combine(saveto, "System.Data.SQLite.dll"));
                File.Copy(Path.Combine(sepath, "VRage.Audio.dll"), Path.Combine(saveto, "VRage.Audio.dll"));
                File.Copy(Path.Combine(sepath, "VRage.dll"), Path.Combine(saveto, "VRage.dll"));
                File.Copy(Path.Combine(sepath, "VRage.Game.dll"), Path.Combine(saveto, "VRage.Game.dll"));
                File.Copy(Path.Combine(sepath, "VRage.Game.XmlSerializers.dll"), Path.Combine(saveto, "VRage.Game.XmlSerializers.dll"));
                File.Copy(Path.Combine(sepath, "VRage.Input.dll"), Path.Combine(saveto, "VRage.Input.dll"));
                File.Copy(Path.Combine(sepath, "VRage.Library.dll"), Path.Combine(saveto, "VRage.Library.dll"));
                File.Copy(Path.Combine(sepath, "VRage.Math.dll"), Path.Combine(saveto, "VRage.Math.dll"));
                File.Copy(Path.Combine(sepath, "VRage.Native.dll"), Path.Combine(saveto, "VRage.Native.dll"));

                System.Windows.Forms.Application.Restart();
                return;
            }
            catch (Exception ex)
            {
                // TODO log error
                MessageBox.ShowError(ex.Message);
                return;
            }
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
    }
}
