﻿using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels.Commands
{
    public class CloseProjectCommand : ICommand
    {
        private readonly ProjectViewModel _Project;

        public CloseProjectCommand(ProjectViewModel project)
        {
            _Project = project;
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
            if (_Project != null)
            {
                _Project.PerformCloseProject();
            }
        }
    }
}
