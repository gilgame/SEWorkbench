using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public delegate void ErrorMessageEventHandler(object sender, ErrorMessageEventArgs e);

    public class ErrorMessageEventArgs : EventArgs
    {
        private OutputItemViewModel _Output;
        public OutputItemViewModel Output
        {
            get
            {
                return _Output;
            }
        }

        public ErrorMessageEventArgs(OutputItemViewModel output)
        {
            _Output = output;
        }
    }
}
