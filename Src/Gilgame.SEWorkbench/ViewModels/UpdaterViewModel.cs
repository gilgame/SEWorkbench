using System;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class UpdaterViewModel : BaseViewModel
    {
        #region Properties

        private string _Location = String.Empty;
        public string Location
        {
            get
            {
                return _Location;
            }
            set
            {
                if (_Location != value)
                {
                    _Location = value;
                    RaisePropertyChanged("Location");
                }
            }
        }

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

        public void PerformCancel(Window window)
        {
            if (!CanCancel)
            {
                return;
            }

            // cancel stuff

            window.Close();
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
            if (!CanDownload)
            {
                return;
            }

            string destination = Services.IO.Path.GetTempFileName();

            // download file, unpack
        }

        #endregion

        #endregion
    }
}
