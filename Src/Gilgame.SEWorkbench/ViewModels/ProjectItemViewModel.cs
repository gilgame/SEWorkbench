using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Gilgame.SEWorkbench.Models;
using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectItemViewModel : BaseViewModel
    {
        private Services.ObservableSortedList<ProjectItemViewModel> _Children;
        public Services.ObservableSortedList<ProjectItemViewModel> Children
        {
            get
            {
                return _Children;
            }
        }

        private ProjectItem _Model;
        public ProjectItem Model
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

        public ProjectItemType Type
        {
            get
            {
                return _Model.Type;
            }
        }

        public string Path
        {
            get
            {
                return _Model.Path;
            }
        }

        public string Blueprint
        {
            get
            {
                return _Model.Blueprint;
            }
        }

        private ObservableSortedList<GridItemViewModel> _Grid;
        public ObservableSortedList<GridItemViewModel> Grid
        {
            get
            {
                return _Grid;
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
                    ProjectItemViewModel parent = (ProjectItemViewModel)Parent;
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

                ProjectItemViewModel parent = (ProjectItemViewModel)Parent;
                if (_IsVisible && parent != null)
                {
                    parent.IsVisible = true;
                }
                if (!_IsVisible && parent != null)
                {
                    parent.IsVisible = false;
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
                    _Model.Project.RaiseSelectionChanged();
                }
            }
        }

        public ProjectItemViewModel(ProjectItem item) : this(item, null)
        {
            _FileRequestedCommand = new Commands.FileRequestedCommand(this);
        }

        public ProjectItemViewModel(ProjectItem item, ProjectItemViewModel parent) : base(parent)
        {
            _Model = item;

            _Children = new Services.ObservableSortedList<ProjectItemViewModel>(
                (from child in _Model.Children select new ProjectItemViewModel(child, this)).ToList<ProjectItemViewModel>(),
                new Comparers.ProjectItemComparer()
            );

            _Grid = new Services.ObservableSortedList<GridItemViewModel>(
                new GridItemViewModel[] { },
                new Comparers.GridItemComparer()
            );
        }

        public void AddChild(ProjectItem item)
        {
            _Children.Add(new ProjectItemViewModel(item, this));
            Model.Children.Add(item);
        }

        public void AddChild(ProjectItem item, Interop.Grid grid)
        {
            ProjectItemViewModel vm = new ProjectItemViewModel(item, this);
            vm.Grid.Add(CreateGridViewModel(grid));

            _Children.Add(vm);
            Model.Children.Add(item);
        }

        public void Remove()
        {
            ProjectItemViewModel parent = (ProjectItemViewModel)Parent;
            parent.RemoveChild(this);
        }

        private void RemoveChild(ProjectItemViewModel child)
        {
            Model.Children.Remove(child.Model);
            _Children.Remove(child);
        }

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(Name))
            {
                return false;
            }
            return Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        public void SetGrid(Interop.Grid grid)
        {
            GridItemViewModel vm = CreateGridViewModel(grid);

            _Grid.Clear();
            _Grid.Add(vm);
        }

        private GridItemViewModel CreateGridViewModel(Interop.Grid grid)
        {
            GridItemViewModel root = new GridItemViewModel(new GridItem() { Name = grid.Name, Type = GridItemType.Root }, grid.Definition, grid.Path);
            foreach(KeyValuePair<string, List<Interop.TerminalBlock>> pair in grid.Blocks)
            {
                GridItemViewModel node = new GridItemViewModel(new GridItem() { Name = pair.Key, Type = GridItemType.Group }, root);
                foreach(Interop.TerminalBlock block in pair.Value)
                {
                    node.AddChild(new GridItemViewModel(new GridItem() { Name = block.Name, Type = GridItemType.Block }, node));
                }
                root.AddChild(node);
            }
            return root;
        }

        #region File Requested Command

        private readonly ICommand _FileRequestedCommand;
        public ICommand FileRequestedCommand
        {
            get
            {
                return _FileRequestedCommand;
            }
        }

        public void RaiseFileRequested()
        {
            _Model.Project.RaiseFileRequested();
        }

        #endregion
    }
}
