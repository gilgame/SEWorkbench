using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class FileRequestedCommand : ICommand
    {
        private readonly ProjectItemViewModel _ProjectItem;

        public FileRequestedCommand(ProjectItemViewModel item)
        {
            _ProjectItem = item;
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
            _ProjectItem.RaiseFileRequested();
        }
    }
}
