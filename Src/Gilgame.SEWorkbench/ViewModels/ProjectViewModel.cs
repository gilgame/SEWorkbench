using System;
using Gilgame.SEWorkbench.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectViewModel
    {
        private IEnumerator<ProjectItemViewModel> _MatchingItemEnumerator;

        private ObservableCollection<ProjectItemViewModel> _First;
        public ObservableCollection<ProjectItemViewModel> First
        {
            get
            {
                return _First;
            }
        }

        private readonly ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand;
            }
        }

        private string _SearchText = String.Empty;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                if (value == _SearchText)
                {
                    return;
                }

                _SearchText = value;
                _MatchingItemEnumerator = null;
            }
        }

        private readonly ProjectItemViewModel _RootItem;
        public ProjectViewModel(ProjectItem root)
        {
            _RootItem = new ProjectItemViewModel(root);

            _First = new ObservableCollection<ProjectItemViewModel>(
                new ProjectItemViewModel[] { 
                    _RootItem
                }
            );

            _SearchCommand = new SearchProjectCommand(this);
        }

        #region SearchProjectCommand

        private class SearchProjectCommand : ICommand
        {
            private readonly ProjectViewModel _Project;

            public SearchProjectCommand(ProjectViewModel project)
            {
                _Project = project;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _Project.PerformSearch();
            }
        }

        private void PerformSearch()
        {
            if (_MatchingItemEnumerator == null || !_MatchingItemEnumerator.MoveNext())
            {
                VerifyMatchingItemEnumerator();
            }

            var item = _MatchingItemEnumerator.Current;
            if (item == null)
            {
                return;
            }

            if (item.Parent != null)
            {
                item.Parent.IsExpanded = true;
            }

            // TODO switch search to filter (hide unmatched items), probably handled by the UI instead

            item.IsSelected = true;
        }

        private void VerifyMatchingItemEnumerator()
        {
            var matches = FindMatches(_SearchText, _RootItem);

            _MatchingItemEnumerator = matches.GetEnumerator();
            if (!_MatchingItemEnumerator.MoveNext())
            {
                // none found, do nothing for now
            }
        }

        private IEnumerable<ProjectItemViewModel> FindMatches(string text, ProjectItemViewModel item)
        {
            if (item.NameContainsText(text))
            {
                yield return item;
            }

            foreach (ProjectItemViewModel child in item.Children)
            {
                foreach (ProjectItemViewModel match in FindMatches(text, child))
                {
                    yield return match;
                }
            }
        }

        #endregion

        #region Add File Command

        private class AddFileCommand : ICommand
        {
            private readonly ProjectViewModel _Project;

            public AddFileCommand(ProjectViewModel project)
            {
                _Project = project;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _Project.PerformAddNewFile();
            }
        }

        private void PerformAddNewFile()
        {
            // TODO add new file logic
        }

        #endregion

        #region Add Existing File Command

        private class AddExistingFileCommand : ICommand
        {
            private readonly ProjectViewModel _Project;

            public AddExistingFileCommand(ProjectViewModel project)
            {
                _Project = project;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _Project.PerformAddExistingFile();
            }
        }

        private void PerformAddExistingFile()
        {
            // TODO add existing file logic
        }

        #endregion

        #region Add Folder Command

        private class AddFolderCommand : ICommand
        {
            private readonly ProjectViewModel _Project;

            public AddFolderCommand(ProjectViewModel project)
            {
                _Project = project;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _Project.PerformAddNewFolder();
            }
        }

        private void PerformAddNewFolder()
        {
            // TODO add new folder logic
        }

        #endregion

        #region Rename Command

        private class RenameCommand : ICommand
        {
            private readonly ProjectViewModel _Project;

            public RenameCommand(ProjectViewModel project)
            {
                _Project = project;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _Project.PerformRename();
            }
        }

        private void PerformRename()
        {
            // TODO rename item logic
        }

        #endregion

        #region Delete Command

        private class DeleteCommand : ICommand
        {
            private readonly ProjectViewModel _Project;

            public DeleteCommand(ProjectViewModel project)
            {
                _Project = project;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _Project.PerformDelete();
            }
        }

        private void PerformDelete()
        {
            // TODO delete item logic
        }

        #endregion
    }
}
