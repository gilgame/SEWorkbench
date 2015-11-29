using Gilgame.SEWorkbench.Models;
using System;
using System.Linq;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ClassItemViewModel : BaseViewModel
    {
        private Services.ObservableSortedList<ClassItemViewModel> _Children;
        public Services.ObservableSortedList<ClassItemViewModel> Children
        {
            get
            {
                return _Children;
            }
        }

        private ClassItem _Model;
        public ClassItem Model
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

        public string Namespace
        {
            get
            {
                return _Model.Namespace;
            }
        }

        public ClassItemType Type
        {
            get
            {
                return _Model.Type;
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
                    ClassItemViewModel parent = (ClassItemViewModel)Parent;
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
                    ((ClassItemViewModel)Parent).IsVisible = true;
                }
                if (!_IsVisible && Parent != null)
                {
                    ((ClassItemViewModel)Parent).IsVisible = false;
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

        public ClassItemViewModel(ClassItem item) : this(item, null)
        {

        }

        public ClassItemViewModel(ClassItem item, ClassItemViewModel parent) : base(parent)
        {
            _Model = item;

            _Children = new Services.ObservableSortedList<ClassItemViewModel>(
                (from child in _Model.Children select new ClassItemViewModel(item, null)).ToList<ClassItemViewModel>(),
                new Comparers.ClassItemComparer()
            );
        }

        public void AddChild(ClassItemViewModel item)
        {
            _Children.Add(item);
            Model.Children.Add(item.Model);
        }
    }
}
