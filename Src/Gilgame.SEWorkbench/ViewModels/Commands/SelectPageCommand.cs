using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class SelectPageCommand : ICommand
    {
        private readonly PageViewModel _Page;

        public SelectPageCommand(PageViewModel page)
        {
            _Page = page;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public void Execute(object parameter)
        {
            if (_Page != null)
            {
                _Page.PerformSelectPage();
            }
        }
    }
}
