using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class SelectFileCommand : ICommand
    {
        private readonly object _ViewModel;

        public SelectFileCommand(object viewmodel)
        {
            _ViewModel = viewmodel;
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
            if (_ViewModel is PageViewModel)
            {
                PageViewModel vm = (PageViewModel)_ViewModel;
                vm.PerformSelectFile();
            }
        }
    }
}
