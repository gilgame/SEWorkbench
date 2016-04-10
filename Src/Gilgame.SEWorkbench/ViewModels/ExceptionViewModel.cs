using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ExceptionViewModel : BaseViewModel
    {
        private string _Details = String.Empty;
        public string Details
        {
            get
            {
                return _Details;
            }
            set
            {
                if (_Details != value)
                {
                    _Details = value;
                    RaisePropertyChanged("Details");
                }
            }
        }

        private bool? _DialogResult = null;
        public bool? DialogResult
        {
            get
            {
                return _DialogResult;
            }
            private set
            {
                if (_DialogResult != value)
                {
                    _DialogResult = value;
                    RaisePropertyChanged("DialogResult");
                }
            }
        }

        public ExceptionViewModel() : base(null)
        {
            _NavigateCommand = new Commands.DelegateCommand(PerformNavigate);
            _CloseCommand = new Commands.DelegateCommand(PerformClose);
        }

        #region Navigate Command

        private readonly ICommand _NavigateCommand;
        public ICommand NavigateCommand
        {
            get
            {
                return _NavigateCommand;
            }
        }

        public void PerformNavigate()
        {
            Process.Start("http://github.com/gilgame/SEWorkbench/issues");
        }

        #endregion

        #region Close Command

        private readonly ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand;
            }
        }

        public void PerformClose()
        {
            DialogResult = true;
        }

        #endregion
    }
}
