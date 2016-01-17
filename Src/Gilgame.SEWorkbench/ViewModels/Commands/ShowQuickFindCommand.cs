using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class ShowQuickFindCommand : ICommand
    {
        private readonly EditorViewModel _Editor;

        public ShowQuickFindCommand(EditorViewModel editor)
        {
            _Editor = editor;
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
            _Editor.PerformShowQuickFind();
        }
    }
}
