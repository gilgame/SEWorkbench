using System;
using System.ComponentModel;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class PageViewModel : INotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _TabName;
        public string TabName { get; private set; }

        public PageViewModel(string tabname)
        {
            _TabName = tabname;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
