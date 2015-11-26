using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public delegate void InsertEventHandler(object sender, InsertEventArgs e);

    public class InsertEventArgs : EventArgs
    {
        private string _Text = String.Empty;
        public string Text
        {
            get
            {
                return _Text;
            }
        }

        public InsertEventArgs(string text)
        {
            _Text = text;
        }
    }
}
