using System;

using Gilgame.SEWorkbench.Services.IO;

namespace Gilgame.SEWorkbench.Services
{
    public class BackupManager
    {
        private string _ProjectName = String.Empty;

        public BackupManager(string project)
        {
            _ProjectName = HashFilename(project);
        }

        public void BackupFile(string original, string contents)
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string backupdir = Path.Combine(appdata, "backup");
            string project = Path.Combine(backupdir, _ProjectName);

            if (!Directory.Exists(project))
            {
                Directory.CreateDirectory(project);
            }

            string backupfile = Path.Combine(project, HashFilename(original));

            File.Write(backupfile, contents);
        }

        public string GetBackup(string original)
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string backupdir = Path.Combine(appdata, "backup");
            string project = Path.Combine(backupdir, _ProjectName);
            string backupfile = Path.Combine(project, HashFilename(original));

            if (File.Exists(backupfile))
            {
                if (!FirstIsNewer(original, backupfile))
                {
                    if (!Compare(original, backupfile))
                    {
                        return File.Read(backupfile);
                    }
                }
            }

            return null;
        }

        private bool FirstIsNewer(string file1, string file2)
        {
            DateTime? date1 = File.LastWriteTime(file1);
            if (date1 == null)
            {
                return false;
            }

            DateTime? date2 = File.LastWriteTime(file2);
            if (date2 == null)
            {
                return false;
            }
            
            return (date1 >= date2);
        }
        
        private bool Compare(string file1, string file2)
        {
            string contents1 = String.Empty;
            if (File.Exists(file1))
            {
                contents1 = File.Read(file1);
            }

            string contents2 = String.Empty;
            if (File.Exists(file2))
            {
                contents1 = File.Read(file2);
            }

            return (contents1 == contents2);
        }

        private string HashFilename(string path)
        {
            return Services.Hasher.Filename(path);
        }
    }
}
