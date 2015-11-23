using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class MenuItemViewModel : BaseViewModel
    {
        private ObservableCollection<MenuItemViewModel> _Children = new ObservableCollection<MenuItemViewModel>();
        public ObservableCollection<MenuItemViewModel> Children
        {
            get
            {
                return _Children;
            }
        }

        private Models.MenuItem _Model;
        public Models.MenuItem Model
        {
            get
            {
                return _Model;
            }
            private set
            {
                _Model = value;
                OnPropertyChanged("Model");
            }
        }

        public string Name
        {
            get
            {
                return _Model.Name;
            }
            set
            {
                if (_Model.Name != value)
                {
                    _Model.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Header
        {
            get
            {
                return _Model.Header;
            }
        }

        private ICommand _Command;
        public ICommand Command
        {
            get
            {
                return _Command;
            }
            private set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        public string _InputGestureText;
        public string InputGestureText
        {
            get
            {
                return _Model.InputGestureText;
            }
            set
            {
                if (_Model.InputGestureText != value)
                {
                    _Model.InputGestureText = value;
                    OnPropertyChanged("InputGestureText");
                }
            }
        }

        private bool _IsEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        public MenuItemViewModel(BaseViewModel parent, string header) : this(parent, header, null)
        {
        }

        public MenuItemViewModel(BaseViewModel parent, string header, ICommand command) : base(parent)
        {
            Models.MenuItem item = new Models.MenuItem()
            {
                Header = header
            };
            Model = item;
            Command = command;
        }

        public void AddChild(MenuItemViewModel item)
        {
            _Children.Add(item);
        }

        public void AddChild(string header)
        {
            AddChild(header, null, null, null);
        }

        public void AddChild(string header, ICommand command)
        {
            AddChild(header, null, null, command);
        }

        public void AddChild(string header, string name, ICommand command)
        {
            AddChild(header, name, null, command);
        }

        public void AddChild(string header, string name, string gesture, ICommand command)
        {
            MenuItemViewModel item = new MenuItemViewModel(this, header, command)
            {
                Name = name,
                InputGestureText = gesture,
            };
            _Children.Add(item);
        }

        public void AddSeparator()
        {
            _Children.Add(null);
        }

        public MenuItemViewModel GetItemByName(string name)
        {
            foreach (MenuItemViewModel child in _Children)
            {
                if (child.Name == name)
                {
                    return child;
                }
                else
                {
                    MenuItemViewModel found = GetItemByName(child, name);
                    if (found == null)
                    {
                        continue;
                    }
                    else
                    {
                        return found;
                    }
                }
            }
            return null;
        }

        private MenuItemViewModel GetItemByName(MenuItemViewModel item, string name)
        {
            foreach (MenuItemViewModel child in item.Children)
            {
                if (child.Name == name)
                {
                    return child;
                }
                else
                {
                    MenuItemViewModel found = GetItemByName(child, name);
                    if (found == null)
                    {
                        continue;
                    }
                    else
                    {
                        return found;
                    }
                }
            }
            return null;
        }
    }
}
