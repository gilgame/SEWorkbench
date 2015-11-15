using System;
using Gilgame.SEWorkbench.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private IEnumerator<ProjectItemViewModel> _MatchingItemEnumerator;
        private IEnumerator<ProjectItemViewModel> _SelectedItemEnumerator;

        public Models.Project Model
        {
            get
            {
                return new Models.Project()
                {
                    Name = _RootItem.Model.Name,
                    RootItem = _RootItem.Model
                };
            }
        }

        private string _Path = @"C:\Users\Tim\Documents\SEWorkbench\";
        public string Path
        {
            get
            {
                return _Path + Model.Name + @"\";
            }
        }

        public string Filename
        {
            get
            {
                return String.Format(@"{0}{1}.seproj", Path, Model.Name);
            }
        }

        private ObservableCollection<ProjectItemViewModel> _First;
        public ObservableCollection<ProjectItemViewModel> First
        {
            get
            {
                return _First;
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

        public ProjectViewModel()
        {
            _SearchCommand = new Commands.SearchCommand(this);

            _AddCommand = new Commands.AddFileCommand(this);
            _AddExistingCommand = new Commands.AddExistingFileCommand(this);
            _AddFolderCommand = new Commands.AddFolderCommand(this);

            _ViewCodeCommand = new Commands.ViewCodeCommand(this);

            _RenameCommand = new Commands.RenameCommand(this);
            _DeleteCommand = new Commands.DeleteCommand(this);
        }

        private ProjectItemViewModel _RootItem;
        public void SetRootItem(ProjectItem root)
        {
            SetProject(root, this);

            _RootItem = new ProjectItemViewModel(root);

            _First = new ObservableCollection<ProjectItemViewModel>(
                new ProjectItemViewModel[] { 
                    _RootItem
                }
            );
        }

        private void SetProject(ProjectItem parent, ProjectViewModel project)
        {
            parent.Project = project;
            foreach (ProjectItem child in parent.Children)
            {
                SetProject(child, project);
            }
        }

        public void SaveProject()
        {
            string serialized = Serialization.Convert.ToSerialized(this.Model);
            serialized = System.Xml.Linq.XDocument.Parse(serialized).ToString() + Environment.NewLine;

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            File.WriteAllText(Filename, serialized);
        }

        public void OpenProject(string path)
        {
            string serialized = File.ReadAllText(path);

            Project project = (Project)Serialization.Convert.ToObject(serialized);

            SetRootItem(project.RootItem);
        }

        public static ProjectViewModel NewProject(string name)
        {
            ProjectViewModel project = new ViewModels.ProjectViewModel();

            ProjectItem root = new ProjectItem()
            {
                Name = name,
                Type = Models.ProjectItemType.Root,
                Path = name,
                Project = project,
            };
            project.SetRootItem(root);

            return project;
        }

        private ProjectItemViewModel SelectedItem
        {
            get
            {
                return FindSelectedItem();
            }
        }

        public Models.ProjectItemType SelectedItemType
        {
            get
            {
                return (SelectedItem == null) ? Models.ProjectItemType.None : SelectedItem.Type;
            }
        }

        private ProjectItemViewModel FindSelectedItem()
        {
            if (_SelectedItemEnumerator == null || !_SelectedItemEnumerator.MoveNext())
            {
                VerifySelectedItemEnumerator();
            }

            return _SelectedItemEnumerator.Current;
        }

        private void VerifySelectedItemEnumerator()
        {
            var matches = FindSelected(_RootItem);

            _SelectedItemEnumerator = matches.GetEnumerator();
            if (!_SelectedItemEnumerator.MoveNext())
            {
                // none selected
            }
        }

        private IEnumerable<ProjectItemViewModel> FindSelected(ProjectItemViewModel item)
        {
            if (item.IsSelected)
            {
                yield return item;
            }

            foreach (ProjectItemViewModel child in item.Children)
            {
                foreach (ProjectItemViewModel match in FindSelected(child))
                {
                    // TODO fix collection modified exception
                    yield return match;
                }
            }
        }

        private ProjectItemViewModel GetParentFolder(ProjectItemViewModel child)
        {
            if (child == null || child.Parent == null)
            {
                return null;
            }

            if (child.Parent.Type == ProjectItemType.Folder)
            {
                return child.Parent;
            }

            return GetParentFolder(child.Parent);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void SelectionChanged()
        {
            OnPropertyChanged("SelectedItemType");
        }

        #region Search Command

        private readonly ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand;
            }
        }

        public void PerformSearch()
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

        private readonly ICommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                return _AddCommand;
            }
        }

        public void PerformAddFile()
        {
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return;
            }

            if (selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }
            if (selected == null)
            {
                return;
            }

            Views.NewItemDialog dialog = new Views.NewItemDialog();
            dialog.ShowDialog();

            string name = String.Empty;
            if (dialog.DialogResult != null && dialog.DialogResult.Value)
            {
                name = dialog.ItemName;
            }

            if (!String.IsNullOrEmpty(name))
            {
                // TODO add to filesystem
                selected.AddChild(new ProjectItem() { Name = name, Type = ProjectItemType.File });
            }
        }

        #endregion

        #region Add Existing File Command

        private readonly ICommand _AddExistingCommand;
        public ICommand AddExistingCommand
        {
            get
            {
                return _AddExistingCommand;
            }
        }

        public void PerformAddExistingFile()
        {
            // TODO add existing file logic
        }

        #endregion

        #region Add Folder Command

        private readonly ICommand _AddFolderCommand;
        public ICommand AddFolderCommand
        {
            get
            {
                return _AddFolderCommand;
            }
        }

        public void PerformAddFolder()
        {
            // TODO add new folder logic
        }

        #endregion

        #region View Code Command

        private readonly ICommand _ViewCodeCommand;
        public ICommand ViewCodeCommand
        {
            get
            {
                return _ViewCodeCommand;
            }
        }

        public void PerformViewCode()
        {
            // TODO add new folder logic
        }

        #endregion

        #region Rename Command

        private readonly ICommand _RenameCommand;
        public ICommand RenameCommand
        {
            get
            {
                return _RenameCommand;
            }
        }

        public void PerformRename()
        {
            // TODO rename item logic
        }

        #endregion

        #region Delete Command

        private readonly ICommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _DeleteCommand;
            }
        }

        public void PerformDelete()
        {
            // TODO delete item logic
        }

        #endregion
    }
}
