using System;

namespace Gilgame.SEWorkbench.ViewModels.Config
{
    public class FontViewModel : BaseViewModel
    {
        private string _Name = String.Empty;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public FontViewModel(BaseViewModel parent) : base(parent)
        {
            // do nothing
        }
    }
}
