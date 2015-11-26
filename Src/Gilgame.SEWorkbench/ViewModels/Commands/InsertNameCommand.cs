using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class InsertNameCommand : ICommand
    {
        private readonly BlueprintViewModel _Blueprint;

        public InsertNameCommand(BlueprintViewModel blueprint)
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
            _Blueprint.PerformInsertName();
        }
    }
}
