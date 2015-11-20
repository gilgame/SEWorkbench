using Gilgame.SEWorkbench.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class GridItemViewModel : INotifyPropertyChanged
    {
        private Services.ObservableSortedList<GridItemViewModel> _Children;
        public Services.ObservableSortedList<GridItemViewModel> Children
        {
            get
            {
                return _Children;
            }
        }

        public string Name
        {
            get
            {
                return _Model.Name;
            }
        }

        private GridItem _Model;
        public GridItem Model
        {
            get
            {
                return _Model;
            }
        }

        private GridItemViewModel _Parent;
        public GridItemViewModel Parent
        {
            get
            {
                return _Parent;
            }
        }

        private bool _IsExpanded = false;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (value != _IsExpanded)
                {
                    _IsExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                if (_IsExpanded && _Parent != null)
                {
                    _Parent.IsExpanded = true;
                }
            }
        }

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                if (value != _IsVisible)
                {
                    _IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }

                if (_IsVisible && _Parent != null)
                {
                    _Parent.IsVisible = true;
                }
                if (!_IsVisible && _Parent != null)
                {
                    _Parent.IsVisible = false;
                }
            }
        }

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
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public GridItemViewModel(GridItem item) : this(item, null)
        {
            // pass it on
        }

        public GridItemViewModel(GridItem item, GridItemViewModel parent)
        {
            _Model = item;
            _Parent = parent;

            _Children = new Services.ObservableSortedList<GridItemViewModel>(
                (from child in _Model.Children select new GridItemViewModel(child, this)).ToList<GridItemViewModel>(),
                new Comparers.GridItemComparer<GridItemViewModel>()
            );
        }

        public void AddChild(GridItemViewModel item)
        {
            _Children.Add(item);
            Model.Children.Add(item.Model);
        }
    }
}
