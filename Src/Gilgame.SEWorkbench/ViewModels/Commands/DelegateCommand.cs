using System;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly object _Action;

        public DelegateCommand(Action action)
        {
            _Action = action;
        }

        public DelegateCommand(Action<string> action)
        {
            _Action = action;
        }

        public DelegateCommand(Action<bool> action)
        {
            _Action = action;
        }

        public DelegateCommand(Action<Window> action)
        {
            _Action = action;
        }

        public void Execute(object parameter)
        {
            if (_Action != null)
            {
                if (_Action is Action<Window>)
                {
                    Action<Window> action = (Action<Window>)_Action;
                    action((Window)parameter);
                }
                if (_Action is Action<string>)
                {
                    Action<string> action = (Action<string>)_Action;
                    action(Convert.ToString(parameter));
                }
                if (_Action is Action<bool>)
                {
                    Action<bool> action = (Action<bool>)_Action;
                    action(Convert.ToBoolean(parameter));
                }
                if (_Action is Action)
                {
                    Action action = (Action)_Action;
                    action();
                }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
