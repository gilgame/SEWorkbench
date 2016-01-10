using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    class RenameBlockCommand : ICommand
    {
        private readonly BlueprintViewModel _Blueprint;

        public RenameBlockCommand(BlueprintViewModel blueprint)
        {
            _Blueprint = blueprint;
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
            if (_Blueprint != null)
            {
                _Blueprint.PerformRenameBlock();
            }
        }
    }
}
