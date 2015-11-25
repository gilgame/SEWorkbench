using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class SearchCommand :ICommand
    {
        private readonly object _ViewModel;

        public SearchCommand(object viewmodel)
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
                vm.PerformSearch();
            }
            if (_ViewModel is BlueprintViewModel)
            {
                BlueprintViewModel vm = (BlueprintViewModel)_ViewModel;
                vm.PerformSearch();
            }
        }
    }
}
