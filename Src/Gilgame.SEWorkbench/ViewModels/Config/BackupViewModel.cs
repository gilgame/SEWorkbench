using System;

namespace Gilgame.SEWorkbench.ViewModels.Config
{
    public class BackupViewModel : BaseViewModel
    {
        public bool Enabled
        {
            get
            {
                return Configuration.Backups.Enabled;
            }
            set
            {
                Configuration.Backups.Enabled = value;
                RaisePropertyChanged("Enabled");
            }
        }

        public int Interval
        {
            get
            {
                return Configuration.Backups.Interval;
            }
            set
            {
                Configuration.Backups.Interval = value;
                RaisePropertyChanged("Interval");
            }
        }

        public BackupViewModel(BaseViewModel parent) : base(parent)
        {

        }
    }
}
