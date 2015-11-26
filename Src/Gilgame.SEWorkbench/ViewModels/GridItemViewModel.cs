using System;
using System.Linq;

using Gilgame.SEWorkbench.Models;
using Sandbox.Common.ObjectBuilders.Definitions;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class GridItemViewModel : BaseViewModel
    {
        private Services.ObservableSortedList<GridItemViewModel> _Children;
        public Services.ObservableSortedList<GridItemViewModel> Children
        {
            get
            {
                return _Children;
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

        public string Name
        {
            get
            {
                return _Model.Name;
            }
        }

        public long EntityID
        {
            get
            {
                return _Model.EntityID;
            }
        }

        public string Program
        {
            get
            {
                return _Model.Program;
            }
            set
            {
                _Model.Program = value;
            }
        }

        public GridItemType Type
        {
            get
            {
                return _Model.Type;
            }
        }

        private MyObjectBuilder_Definitions _Definitions;
        public MyObjectBuilder_Definitions Definitions
        {
            get
            {
                return _Definitions;
            }
            set
            {
                _Definitions = value;
            }
        }

        private string _Path;
        public string Path
        {
            get
            {
                return _Path;
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

                if (_IsExpanded && Parent != null)
                {
                    GridItemViewModel parent = (GridItemViewModel)Parent;
                    parent.IsExpanded = true;
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

                if (_IsVisible && Parent != null)
                {
                    ((GridItemViewModel)Parent).IsVisible = true;
                }
                if (!_IsVisible && Parent != null)
                {
                    ((GridItemViewModel)Parent).IsVisible = false;
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

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(Name))
            {
                return false;
            }
            return Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        public GridItemViewModel(GridItem item, MyObjectBuilder_Definitions definition, string path) : this(item, null)
        {
            _Definitions = definition;
            _Path = path;
        }

        public GridItemViewModel(GridItem item, GridItemViewModel parent) : base(parent)
        {
            _Model = item;

            _Children = new Services.ObservableSortedList<GridItemViewModel>(
                (from child in _Model.Children select new GridItemViewModel(item, null)).ToList<GridItemViewModel>(),
                new Comparers.GridItemComparer()
            );
        }

        public void AddChild(GridItemViewModel item)
        {
            _Children.Add(item);
            Model.Children.Add(item.Model);
        }
    }
}
