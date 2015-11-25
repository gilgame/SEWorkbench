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

        public FileEventArgs(string path)
        {
            _Path = path;
        }
    }
}
