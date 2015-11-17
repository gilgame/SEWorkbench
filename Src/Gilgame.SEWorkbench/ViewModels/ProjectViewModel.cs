using System;
using Gilgame.SEWorkbench.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Gilgame.SEWorkbench.Services;

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
                    Path = _RootItem.Path,
                    RootItem = _RootItem.Model,
                };
            }
        }

        private ObservableSortedList<ProjectItemViewModel> _First;
        public ObservableSortedList<ProjectItemViewModel> First
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
            _AddBlueprintsCommand = new Commands.AddBlueprintsCommand(this);
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

            _First = new Services.ObservableSortedList<ProjectItemViewModel>(
                new ProjectItemViewModel[] { _RootItem },
                new Comparers.ProjectItemComparer<ProjectItemViewModel>()
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
            string serialized = Serialization.Convert.ToSerialized(Model);

            // make it human readable
            serialized = System.Xml.Linq.XDocument.Parse(serialized).ToString() + Environment.NewLine;

            string directory = Path.GetDirectoryName(Model.Path);
            string filename = Model.Path;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filename, serialized);
        }

        public void OpenProject()
        {
            //string serialized = File.ReadAllText(path);

            //Project project = (Project)Serialization.Convert.ToObject(serialized);

            //SetRootItem(project.RootItem);

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string initial = String.Format("{0}{1}{2}", documents, Path.DirectorySeparatorChar, "SEWorkbench");

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".seproj",
                Filter = "SE Workbench Project File (.seproj)|*.seproj",
                Multiselect = false,
                InitialDirectory = initial,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string fullpath = dialog.FileName;
                string serialized = File.ReadAllText(fullpath);

                Project project = (Project)Serialization.Convert.ToObject(serialized);
                SetRootItem(project.RootItem);

                LoadBlueprints();
            }
        }

        private void LoadBlueprints()
        {
            Interop.Blueprint.RunInit();

            LoadBlueprints(_RootItem);
        }

        private void LoadBlueprints(ProjectItemViewModel parent)
        {
            if (parent.Type == ProjectItemType.Blueprints)
            {
                string name;
                Interop.GridTerminalSystem grid;

                Interop.Blueprint.Import(parent.Blueprint, out name, out grid);

                parent.Grid = grid;
            }
            else
            {
                foreach(ProjectItemViewModel child in parent.Children)
                {
                    LoadBlueprints(child);
                }
            }
        }

        public static ProjectViewModel NewProject()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string initial = String.Format("{0}{1}{2}", documents, Path.DirectorySeparatorChar, "SEWorkbench");

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = ".seproj",
                Filter = "SE Workbench Project File (.seproj)|*.seproj",
                InitialDirectory = initial,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string fullpath = dialog.FileName;
                string name = Path.GetFileNameWithoutExtension(fullpath);

                ProjectViewModel project = new ViewModels.ProjectViewModel();

                ProjectItem root = new ProjectItem()
                {
                    Name = name,
                    Type = Models.ProjectItemType.Root,
                    Path = fullpath,
                    Project = project,
                };
                project.SetRootItem(root);

                return project;
            }

            return null;
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
            VerifySelectedItemEnumerator();

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

            if (child.Parent.Type == ProjectItemType.Blueprints || child.Parent.Type == ProjectItemType.Folder)
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

            if (selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }
            if (selected == null)
            {
                return;
            }

            string initial = selected.Path;

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = ".csx",
                Filter = "Script File (.csx)|*.csx",
                InitialDirectory = initial,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string fullpath = dialog.FileName;
                string name = Path.GetFileNameWithoutExtension(fullpath);

                if (!File.Exists(fullpath))
                {
                    File.Create(fullpath);
                }

                if (File.Exists(fullpath))
                {
                    File.WriteAllText(fullpath, Services.NewFile.Contents);

                    ProjectItem item = new ProjectItem()
                    {
                        Name = name,
                        Path = fullpath,
                        Type = ProjectItemType.File,
                        Project = this,
                    };
                    selected.AddChild(item);
                    selected.IsExpanded = true;
                }
                SaveProject();
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
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return;
            }

            if (selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }
            if (selected == null)
            {
                return;
            }

            string initial = selected.Path;

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".csx",
                Filter = "Script File (.csx)|*.csx",
                InitialDirectory = initial,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                foreach (string fullpath in dialog.FileNames)
                {
                    string name = Path.GetFileNameWithoutExtension(fullpath);

                    ProjectItem item = new ProjectItem()
                    {
                        Name = name,
                        Path = fullpath,
                        Type = ProjectItemType.File,
                        Project = this,
                    };
                    selected.AddChild(item);
                    selected.IsExpanded = true;
                }
                SaveProject();
            }
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
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                selected = _RootItem;
            }

            if (selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }
            if (selected == null)
            {
                return;
            }

            Views.NewItemDialog dialog = new Views.NewItemDialog();

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string name = dialog.ItemName;
                string path = String.Format("{0}{1}{2}", selected.Path, Path.DirectorySeparatorChar, name);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                ProjectItem item = new ProjectItem()
                {
                    Name = name,
                    Path = path,
                    Type = ProjectItemType.Folder,
                    Project = this,
                };
                selected.AddChild(item);
                selected.IsExpanded = true;

                SaveProject();
            }
        }

        #endregion

        #region Add Blueprints Command

        private readonly ICommand _AddBlueprintsCommand;
        public ICommand AddBlueprintsCommand
        {
            get
            {
                return _AddBlueprintsCommand;
            }
        }

        public void PerformAddBlueprints()
        {
            ProjectItemViewModel rootitem = _RootItem;
            if (rootitem == null)
            {
                return;
            }

            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string blueprints = String.Format("{0}{1}SpaceEngineers{1}Blueprints", appdata, Path.DirectorySeparatorChar);

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".sbc",
                Filter = "Blueprints File (.sbc)|*.sbc",
                Multiselect = false,
                InitialDirectory = blueprints,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string fullpath = dialog.FileName;

                string name;
                Interop.GridTerminalSystem grid;

                // TODO gonna need to import on another thread
                Interop.Blueprint.RunInit();
                Interop.Blueprint.Import(fullpath, out name, out grid);

                if (grid != null)
                {
                    string rootpath = Path.GetDirectoryName(rootitem.Path);
                    string savepath = String.Format("{0}{1}{2}", rootpath, Path.DirectorySeparatorChar, name);

                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }

                    ProjectItem item = new ProjectItem()
                    {
                        Name = name,
                        Path = savepath,
                        Blueprint = fullpath,
                        Type = ProjectItemType.Blueprints,
                        Project = this,
                    };
                    rootitem.AddChild(item, grid);
                    rootitem.IsExpanded = true;

                    SaveProject();
                }
            }
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
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null || selected.Type == ProjectItemType.Root)
            {
                return;
            }

            string message = String.Format("'{0}' will be deleted permanantly?", selected.Name);

            //var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            var result = MessageBox.ShowQuestion(message);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                bool success = false;
                try
                {
                    switch(selected.Type)
                    {
                        case ProjectItemType.Blueprints:
                        case ProjectItemType.Folder:
                            Directory.Delete(selected.Path, true);
                            break;

                        case ProjectItemType.File:
                            File.Delete(selected.Path);
                            break;

                        default:
                            MessageBox.ShowError("Deleting this object is not supported or you have reached this message in error.");
                            break;
                    }
                    success = true;
                }
                catch (Exception)
                {
                    string error = String.Format("Unable to delete ({0}). Make sure you have permission and that the file is not read-only.", selected.Path);
                    MessageBox.ShowError(error);
                }
                finally
                {
                    if (success)
                    {
                        selected.Remove();
                    }

                    SaveProject();
                }
            }
        }

        #endregion
    }
}
