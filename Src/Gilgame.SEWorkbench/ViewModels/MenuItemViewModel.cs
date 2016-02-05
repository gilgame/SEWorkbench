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

        public Guid Identifier
        {
            get
            {
                return _Model.Identifier;
            }
            set
            {
                _Model.Identifier = value;
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
                RaisePropertyChanged("Model");
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
                    RaisePropertyChanged("Name");
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
                    RaisePropertyChanged("Command");
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
                    RaisePropertyChanged("InputGestureText");
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
                    RaisePropertyChanged("IsEnabled");
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

        public void AddSeparator()
        {
            _Children.Add(null);
        }

        public void RemoveChildByIdentifier(Guid identifier)
        {
            foreach (MenuItemViewModel child in Children)
            {
                if (child == null)
                {
                    continue;
                }

                if (child.Identifier == identifier)
                {
                    Children.Remove(child);
                    break;
                }
                RemoveChildByIdentifier(identifier, child);
            }
        }

        private void RemoveChildByIdentifier(Guid identifier, MenuItemViewModel root)
        {
            foreach (MenuItemViewModel child in root.Children)
            {
                if (child == null)
                {
                    continue;
                }

                if (child.Identifier == identifier)
                {
                    Children.Remove(child);
                    break;
                }
                RemoveChildByIdentifier(identifier, child);
            }
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
