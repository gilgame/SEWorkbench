using System;

namespace Gilgame.SEWorkbench.ViewModels.Config
{
    public class UpdaterViewModel : BaseViewModel
    {
        public bool CheckForUpdates
        {
            get
            {
                return Configuration.Program.CheckForUpdates;
            }
            set
            {
                Configuration.Program.CheckForUpdates = value;
                RaisePropertyChanged("CheckForUpdates");
            }
        }

        public UpdaterViewModel(BaseViewModel parent) : base(parent)
        {

        }
    }
}
