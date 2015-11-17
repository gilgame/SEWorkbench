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

        public ViewModels.EditorViewModel Editor { get; set; }

        private bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                    
                    if (Editor != null)
                    {
                        Editor.SelectionChanged();
                    }
                }
            }
        }

        public PageViewModel(string name)
        {
            _Name = name;
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
