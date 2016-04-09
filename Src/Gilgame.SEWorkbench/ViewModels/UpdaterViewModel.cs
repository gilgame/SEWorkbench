using System;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class UpdaterViewModel : BaseViewModel
    {
        #region Properties

        private string _Details = String.Empty;
        public string Details
        {
            get
            {
                return _Details;
            }
            private set
            {
                if (_Details != value)
                {
                    _Details = value;
                    RaisePropertyChanged("Details");
                }
            }
        }

        private int _Progress = 0;
        public int Progress
        {
            get
            {
                return _Progress;
            }
            private set
            {
                if (_Progress != value)
                {
                    _Progress = value;
                    RaisePropertyChanged("Progress");
                }
            }
        }

        private bool _CanCancel = true;
        public bool CanCancel
        {
            get
            {
                return _CanCancel;
            }
            private set
            {
                if (_CanCancel != value)
                {
                    _CanCancel = value;
                    RaisePropertyChanged("CanCancel");
                }
            }
        }

        private bool _CanDownload = true;
        public bool CanDownload
        {
            get
            {
                return _CanDownload;
            }
            private set
            {
                if (_CanDownload != value)
                {
                    _CanDownload = value;
                    RaisePropertyChanged("CanDownload");
                }
            }
        }

        #endregion

        public UpdaterViewModel() : base(null)
        {
            _CancelCommand = new Commands.DelegateCommand(PerformCancel);
            _DownloadCommand = new Commands.DelegateCommand(PerformDownload);
        }

        #region Commands

        #region Cancel Command

        private readonly ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand;
            }
        }

        public void PerformCancel()
        {

        }

        #endregion

        #region DownloadCommand

        private readonly ICommand _DownloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                return _DownloadCommand;
            }
        }

        public void PerformDownload()
        {
            
        }

        #endregion

        #endregion
    }
}
