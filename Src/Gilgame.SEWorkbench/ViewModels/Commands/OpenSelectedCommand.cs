using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class OpenSelectedCommand : ICommand
    {
        private readonly ProjectManagerViewModel _ProjectManager;

        public OpenSelectedCommand(ProjectManagerViewModel manager)
        {
            _ProjectManager = manager;
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
            if (_ProjectManager != null)
            {
                _ProjectManager.PerformOpenSelected();
            }
        }
    }
}
