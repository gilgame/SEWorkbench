using System;
using System.Collections.ObjectModel;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectManagerViewModel : BaseViewModel
    {
        private string _ProjectName = "SomeProject.seproj";
        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }
            private set
            {
                if (_ProjectName != value)
                {
                    _ProjectName = value;
                    OnPropertyChanged("ProjectName");
                }
            }
        }

        private ObservableCollection<MenuItemViewModel> _RootMenuItem;
        public ObservableCollection<MenuItemViewModel> RootMenuItem
        {
            get
            {
                return _RootMenuItem;
            }
        }

        private ObservableCollection<PageViewModel> _TabItems = new ObservableCollection<PageViewModel>();
        public ObservableCollection<PageViewModel> TabItems
        {
            get
            {
                return _TabItems;
            }
        }

        private ProjectViewModel _Project;
        public ProjectViewModel Project
        {
            get
            {
                return _Project;
            }
            private set
            {
                _Project = value;
                OnPropertyChanged("Project");
            }
        }

        private ProjectItemViewModel _Blueprint;
        public ProjectItemViewModel Blueprint
        {
            get
            {
                return _Blueprint;
            }
            private set
            {
                _Blueprint = value;
                OnPropertyChanged("Blueprint");
            }
        }

        public ProjectManagerViewModel(BaseViewModel parent) : base(parent)
        {
            BuildMenu();

            _Project = new ProjectViewModel();

            _TabItems.Add(new PageViewModel(this, "Test1", null));
        }

        #region BuildMenu

        private void BuildMenu()
        {
            MenuItemViewModel file = new MenuItemViewModel(this, "File");
            {
                MenuItemViewModel mnew = new MenuItemViewModel(file, "New");
                {
                    mnew.AddChild(new MenuItemViewModel(mnew, "Project...") { InputGestureText = "Ctrl+Shift+N" });
                    mnew.AddChild(new MenuItemViewModel(mnew, "File...") { InputGestureText = "Ctrl+N" });
                }
                file.AddChild(mnew);

                MenuItemViewModel mopen = new MenuItemViewModel(file, "Open");
                {
                    mopen.AddChild(new MenuItemViewModel(mopen, "Project...") { InputGestureText = "Ctrl+Shift+O" });
                }
                file.AddChild(mopen);

                file.AddSeparator();

                file.AddChild(new MenuItemViewModel(file, "Close"));

                file.AddSeparator();

                file.AddChild(new MenuItemViewModel(file, "Save") { InputGestureText = "Ctrl+S" });
                file.AddChild(new MenuItemViewModel(file, "Save All") { InputGestureText = "Ctrl+Shift+S" });

                file.AddSeparator();

                file.AddChild(new MenuItemViewModel(file, "Exit") { InputGestureText = "Alt+F4" });
            }

            MenuItemViewModel edit = new MenuItemViewModel(this, "Edit");
            {
                edit.AddChild(new MenuItemViewModel(edit, "Undo") { InputGestureText = "Ctrl+Z" });
                edit.AddChild(new MenuItemViewModel(edit, "Redo") { InputGestureText = "Ctrl+Y" });

                edit.AddSeparator();

                edit.AddChild(new MenuItemViewModel(edit, "Cut") { InputGestureText = "Ctrl+X" });
                edit.AddChild(new MenuItemViewModel(edit, "Copy") { InputGestureText = "Ctrl+C" });
                edit.AddChild(new MenuItemViewModel(edit, "Paste") { InputGestureText = "Ctrl+V" });
                edit.AddChild(new MenuItemViewModel(edit, "Delete") { InputGestureText = "Del" });
                edit.AddChild(new MenuItemViewModel(edit, "Select All") { InputGestureText = "Ctrl+A" });

                edit.AddSeparator();

                edit.AddChild(new MenuItemViewModel(edit, "Settings"));
            }

            MenuItemViewModel project = new MenuItemViewModel(this, "Project");
            {
                project.AddChild(new MenuItemViewModel(project, "Run Script") { InputGestureText = "F5" });
            }

            MenuItemViewModel window = new MenuItemViewModel(this, "Window");
            {
                window.AddChild(new MenuItemViewModel(window, "Close All"));
                window.AddSeparator();
            }

            MenuItemViewModel help = new MenuItemViewModel(this, "Help");
            {
                help.AddChild(new MenuItemViewModel(help, "About"));
            }

            _RootMenuItem = new ObservableCollection<MenuItemViewModel>(
                new MenuItemViewModel[] { file, edit, project, window, help }
            );
        }

        #endregion
    }
}
