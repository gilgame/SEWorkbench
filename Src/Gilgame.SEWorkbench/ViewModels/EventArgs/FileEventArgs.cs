using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public delegate void FileEventHandler(object sender, FileEventArgs e);

    public class FileEventArgs : EventArgs
    {
        private string _Path = String.Empty;
        public string Path
        {
            get
            {
                return _Path;
            }
        }

        private bool _Unsaved = false;
        public bool Unsaved
        {
            get
            {
                return _Unsaved;
            }
        }

        public FileEventArgs(string path)
        {
            _Path = path;
        }

        public FileEventArgs(string path, bool unsaved)
        {
            _Path = path;
            _Unsaved = unsaved;
        }
    }
}
