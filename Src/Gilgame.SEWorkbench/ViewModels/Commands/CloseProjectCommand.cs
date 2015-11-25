using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class CloseProjectCommand : ICommand
    {
        private readonly object _ViewModel;

        public CloseProjectCommand(object viewmodel)
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
            if (_ViewModel is ProjectViewModel)
            {
                ProjectViewModel vm = (ProjectViewModel)_ViewModel;
                vm.PerformCloseProject();
            }
            if (_ViewModel is ProjectManagerViewModel)
            {
                ProjectManagerViewModel vm = (ProjectManagerViewModel)_ViewModel;
                vm.PerformCloseProject();
            }
        }
    }
}
