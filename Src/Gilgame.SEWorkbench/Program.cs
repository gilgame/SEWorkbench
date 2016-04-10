﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;

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
            if (Configuration.Program.CheckForUpdates)
            {
                Models.Update update = CheckForUpdate();
                if (update.IsNewer)
                {
                    Views.UpdaterView updater = new Views.UpdaterView();

                    ViewModels.UpdaterViewModel context = (ViewModels.UpdaterViewModel)updater.DataContext;
                    context.Location = update.Location;
                    context.Details = update.Details;
                    context.CheckSum = update.CheckSum;
                    
                    if (updater.ShowDialog() == true)
                    {
                        string temp = context.ExtractedPath;
                        string working = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                        UpdateProgram(temp, working);

                        return;
                    }
                }
            }
            
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
                    string message = "SE Workbench needs to copy the sandbox files. Make sure Space Engineers and Steam are closed and then press OK to continue.";
                    MessageBox.ShowMessage(message);

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

            Interop.Engine.Initialize();
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

        private static bool SandboxIsCopied(string sepath)
        {
            string local = Directory.GetCurrentDirectory();
            foreach (string assembly in Interop.Engine.Dependencies)
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
                foreach (string assembly in Interop.Engine.Dependencies)
                {
                    CopyFile(Path.Combine(source, assembly), Path.Combine(destination, assembly));
                }

                return true;
            }
            catch (Exception ex)
            {
                // TODO log errors
                MessageBox.ShowError("Failed to copy the required libraries. This can occur if Space Engineers and/or Steam is currently running", ex);
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
                if (Environment.Is64BitProcess)
                {
                    sepath = Path.Combine(sepath, "Bin64");
                }
                else
                {
                    sepath = Path.Combine(sepath, "Bin");
                }
            }
            return sepath;
        }

        private static string UserGetPath()
        {
            Services.MessageBox.ShowMessage("SE Workbench was unable to locate Space Engineers. You will now be prompted to locate SpaceEngineers.exe manually. Browse to Bin for 32-bit or Bin64 for 64-bit architectures.");
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".exe",
                Filter = "Space Engineers Executable (SpaceEngineers.exe)|SpaceEngineers.exe",
                InitialDirectory = @"C:\Program Files (x86)\Steam\SteamApps\common\SpaceEngineers",
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

        private static Models.Update CheckForUpdate()
        {
            string source = "https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Src/update.xml";

            string contents = String.Empty;
            using (WebClient client = new WebClient())
            {
                contents = client.DownloadString(source);
            }

            Models.Update update = new Models.Update();
            if (!String.IsNullOrEmpty(contents))
            {
                update = (Models.Update)Serialization.Convert.ToObject(contents);

                Version current = Assembly.GetExecutingAssembly().GetName().Version;

                Version remote = null;
                Version.TryParse(update.Version, out remote);

                int compared = current.CompareTo(remote);
                if (compared < 0)
                {
                    update.IsNewer = true;
                }
                else
                {
                    update.IsNewer = false;
                }
            }
            else
            {
                update.IsNewer = false;
            }
            return update;
        }

        private static void UpdateProgram(string temp, string working)
        {
            StringBuilder arguments = new StringBuilder();

            arguments.Append("/C Choice /C Y /N /D Y /T 3 & ");
            arguments.Append(String.Format("del /F /Q /S \"{0}\" & ", working));
            arguments.Append(String.Format("xcopy /E /C /Y /I \"{0}\" \"{1}\" & ", temp, working));
            arguments.Append(String.Format("start \"\" /D \"{0}\" \"{1}\"", working, Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location)));

            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = arguments.ToString();
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.FileName = "cmd.exe";

            Process.Start(info);
        }
    }
}
