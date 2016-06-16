using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public delegate void BackupRequestedEventHandler(object sender, BackupRequestedEventArgs e);

    public class BackupRequestedEventArgs : System.ComponentModel.CancelEventArgs
    {
        private string _contents;
        public string Contents
        {
            get { return _contents; }
        }

        private string _original;
        public string Path
        {
            get { return _original; }
        }

        public BackupRequestedEventArgs(string original, string contents)
        {
            _original = original;
            _contents = contents;
        }

        
    }
}
