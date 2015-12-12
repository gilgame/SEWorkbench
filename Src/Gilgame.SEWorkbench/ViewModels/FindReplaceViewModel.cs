using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class FindReplaceViewModel : BaseViewModel
    {
        private Models.FindMode _Mode = Models.FindMode.Find;
        public Models.FindMode Mode
        {
            get
            {
                return _Mode;
            }
            set
            {
                if (_Mode != value)
                {
                    _Mode = value;
                    OnPropertyChanged("Mode");
                }
            }
        }

        public bool IsFindMode
        {
            get
            {
                return (Mode == Models.FindMode.Find);
            }
            set
            {
                if (value != (Mode == Models.FindMode.Find))
                {
                    Mode = (Mode == Models.FindMode.Find) ? Models.FindMode.Replace : Models.FindMode.Find;
                    OnPropertyChanged("IsFindMode");
                }
            }
        }

        public bool IsReplaceMode
        {
            get
            {
                return (Mode == Models.FindMode.Replace);
            }
            set
            {
                if (value != (Mode == Models.FindMode.Replace))
                {
                    Mode = (Mode == Models.FindMode.Find) ? Models.FindMode.Replace : Models.FindMode.Find;
                    OnPropertyChanged("IsFindMode");
                }
            }
        }

        public FindReplaceViewModel(BaseViewModel parent) : base(parent)
        {

        }
    }
}
