using Gilgame.SEWorkbench.Services.IO;
using System;
using System.ComponentModel;
using System.IO.Compression;
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

        private int _DownloadProgress = 0;
        public int DownloadProgress
        {
            get
            {
                return _DownloadProgress;
            }
            private set
            {
                if (_DownloadProgress != value)
                {
                    _DownloadProgress = value;
                    RaisePropertyChanged("DownloadProgress");
                }
            }
        }

        private int _ExtractProgress = 0;
        public int ExtractProgress
        {
            get
            {
                return _ExtractProgress;
            }
            private set
            {
                if (_ExtractProgress != value)
                {
                    _ExtractProgress = value;
                    RaisePropertyChanged("ExtractProgress");
                }
            }
        }

        private int _ZippedItems = 100;
        public int ZippedItems
        {
            get
            {
                return _ZippedItems;
            }
            private set
            {
                if (_ZippedItems != value)
                {
                    _ZippedItems = value;
                    RaisePropertyChanged("ZippedItems");
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

        private string _ExtractedPath = String.Empty;
        public string ExtractedPath
        {
            get
            {
                return _ExtractedPath;
            }
            private set
            {
                if (_ExtractedPath != value)
                {
                    _ExtractedPath = value;
                    RaisePropertyChanged("ExtractedPath");
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
            if (!CanCancel)
            {
                return;
            }

            _WebClient.CancelAsync();
            _CancelExtract = true;
            
            DialogResult = false;
        }

        #endregion

        #region DownloadCommand
        
        private WebClient _WebClient = new WebClient();
        private bool _CancelExtract = false;

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
            CanDownload = false;

            Filename = Services.IO.Path.GetTempFileName();

            DownloadFile(Location);
        }

        private void DownloadFile(string url)
        {
            DownloadProgress = 0;

            _WebClient.DownloadProgressChanged += Client_DownloadProgressChanged;
            _WebClient.DownloadFileCompleted += Client_DownloadCompleted;

            _WebClient.DownloadFileAsync(new Uri(url), Filename);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgress = e.ProgressPercentage;
        }

        private void Client_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string hash = String.Empty;
                if (Services.IO.File.Exists(Filename))
                {
                    hash = Services.Hasher.File(Filename);
                    if (hash == CheckSum)
                    {
                        ExtractFiles(Filename);
                    }
                }
            }
        }

        private void ExtractFiles(string filename)
        {
            if (Services.IO.File.Exists(filename))
            {
                System.Threading.Thread worker = new System.Threading.Thread
                (
                    delegate ()
                    {
                        ExtractFilesTask(filename);
                    }
                );
                worker.IsBackground = true;
                worker.Start();
            }
        }

        private void ExtractFilesTask(string filename)
        {
            ZipArchive zip = ZipFile.OpenRead(filename);

            ExtractProgress = 0;
            ZippedItems = zip.Entries.Count;

            string temp = Path.Combine(Path.GetDirectoryName(filename), Path.GetRandomFileName());
            Directory.CreateDirectory(temp);

            string root = String.Empty;
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (_CancelExtract)
                {
                    return;
                }

                if (String.IsNullOrEmpty(entry.Name))
                {
                    string dir = Path.Combine(temp, entry.FullName.TrimEnd(new char[] { '/' }));
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (String.IsNullOrEmpty(root))
                    {
                        root = dir;
                    }
                }
                else
                {
                    entry.ExtractToFile(Path.Combine(temp, entry.FullName));

                    ExtractProgress = ExtractProgress + 1;
                }
            }

            ExtractedPath = root;

            DialogResult = true;
        }

        #endregion

        #endregion
    }
}
