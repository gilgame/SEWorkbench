using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class EditProgramCommand : ICommand
    {
        private readonly BlueprintViewModel _Blueprint;

        public EditProgramCommand(BlueprintViewModel blueprint)
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
            _Blueprint.PerformEditProgram();
        }
    }
}
