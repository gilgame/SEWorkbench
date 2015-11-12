using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Gilgame.SEWorkbench.Models
{
    public class ProjectModel : INotifyPropertyChanged
    {
        private Regex _CleanRegex = new Regex("[^a-zA-Z0-9 _-]");

        private char[] _UnsafeChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '_', '-' };

        private string _ProjectName = "Project";
        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }
            set
            {
                if (_ProjectName != value)
                {
                    _ProjectName = value;
                    NotifyPropertyChanged("ProjectName");
                }
            }
        }

        public string WorkingDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void CreateSafeName(out string name)
        {
            name = _ProjectName;
            name = _CleanRegex.Replace(name, "");
            name = name.TrimStart(_UnsafeChars);
            name = String.IsNullOrEmpty(_ProjectName) ? "Project" : name;

            string safe = name;

            int i = 1;
            while (ProjectFolderExists(name))
            {
                name = safe + i.ToString();

                i++;
            }
        }

        private bool ProjectFolderExists(string name)
        {
            return Directory.Exists(WorkingDirectory + "\\" + name);
        }
    }
}
