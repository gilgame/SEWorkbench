using System;
using System.ComponentModel;
using System.Net;
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

        private string _Filename = String.Empty;
        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                if (_Filename != value)
                {
                    _Filename = value;
                    RaisePropertyChanged("Filename");
                }
            }
        }

        private string _CheckSum = String.Empty;
        public string CheckSum
        {
            get
            {
                return _CheckSum;
            }
            set
            {
                if (_CheckSum != value)
                {
                    _CheckSum = value;
                    RaisePropertyChanged("CheckSum");
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

            _WebClient.CancelAsync();
            
            window.Close();
        }

        #endregion

        #region DownloadCommand

        private Window _Parent = null;
        private WebClient _WebClient = new WebClient();

        private readonly ICommand _DownloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                return _DownloadCommand;
            }
        }

        public void PerformDownload(Window window)
        {
            if (!CanDownload)
            {
                return;
            }
            CanDownload = false;

            _Parent = window;

            Filename = Services.IO.Path.GetTempFileName();

            DownloadFile(Location);
        }

        public void DownloadFile(string url)
        {
            Progress = 0;

            _WebClient.DownloadProgressChanged += Client_DownloadProgressChanged;
            _WebClient.DownloadFileCompleted += Client_DownloadCompleted;

            _WebClient.DownloadFileAsync(new Uri(url), Filename);
        }

        public void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }

        public void Client_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string hash = String.Empty;
                if (Services.IO.File.Exists(Filename))
                {
                    hash = Services.Hasher.File(Filename);
                    if (hash == CheckSum)
                    {
                        _Parent.DialogResult = true;
                    }
                }
            }
            _Parent.Close();
        }

        #endregion

        #endregion
    }
}
