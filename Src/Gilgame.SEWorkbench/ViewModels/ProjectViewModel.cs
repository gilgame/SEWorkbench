using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Gilgame.SEWorkbench.Models;
using Gilgame.SEWorkbench.Services;
using Gilgame.SEWorkbench.Services.IO;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectViewModel : BaseViewModel
    {
        private IEnumerator<ProjectItemViewModel> _MatchingItemEnumerator;
        private IEnumerator<ProjectItemViewModel> _SelectedItemEnumerator;

        private bool _OpenState = false;
        public bool OpenState
        {
            get
            {
                return _OpenState;
            }
            set
            {
                _OpenState = value;
            }
        }

        public Models.Project Model
        {
            get
            {
                return new Models.Project()
                {
                    Name = _RootItem.Model.Name,
                    Path = _RootItem.Path,
                    RootItem = _RootItem.Model,
                    OpenState = _OpenState,
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

        public ProjectItemViewModel SelectedItem
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

        public event EventHandler ProjectCreated;
        private void RaiseProjectCreated()
        {
            RaisePropertyChanged("IsClosed");
            if (ProjectCreated != null)
            {
                ProjectCreated(this, EventArgs.Empty);
            }
        }

        public event EventHandler ProjectOpened;
        private void RaiseProjectOpened()
        {
            RaisePropertyChanged("IsClosed");
            if (ProjectOpened != null)
            {
                ProjectOpened(this, EventArgs.Empty);
            }
        }

        public bool IsClosed
        {
            get
            {
                return _RootItem == null;
            }
        }

        public event EventHandler ProjectClosed;
        private void RaiseProjectClosed()
        {
            RaisePropertyChanged("IsClosed");
            if (ProjectClosed != null)
            {
                ProjectClosed(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectionChanged;
        public void RaiseSelectionChanged()
        {
            RaisePropertyChanged("SelectedItemType");

            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                selected = _RootItem;
            }

            if (selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }

            SelectionChanged(this, EventArgs.Empty);
        }

        public event EventHandler FileRequested;
        public void RaiseFileRequested()
        {
            if (FileRequested != null)
            {
                FileRequested(this, EventArgs.Empty);
            }
        }

        public event FileEventHandler FileCreated;
        public void RaiseFileCreated(ProjectItemViewModel item)
        {
            if (item.Type == ProjectItemType.File)
            {
                if (FileCreated != null)
                {
                    FileCreated(this, new FileEventArgs(item.Path));
                }
            }
        }

        public event FileEventHandler ReferenceAdded;
        public void RaiseReferenceAdded(ProjectItemViewModel item)
        {
            if (item.Type == ProjectItemType.Reference)
            {
                if (ReferenceAdded != null)
                {
                    ReferenceAdded(this, new FileEventArgs(item.Path));
                }
            }
        }

        public event FileEventHandler FileDeleted;
        public void RaiseFileDeleted(ProjectItemViewModel item)
        {
            if (item.Type == ProjectItemType.File)
            {
                if (FileDeleted != null)
                {
                    FileDeleted(this, new FileEventArgs(item.Path));
                }
            }
            foreach (ProjectItemViewModel child in item.Children)
            {
                RaiseFileDeleted(child);
            }
        }

        public ProjectViewModel(BaseViewModel parent) : base(parent)
        {

            _First = new Services.ObservableSortedList<ProjectItemViewModel>(
                new ProjectItemViewModel[] { },
                new Comparers.ProjectItemComparer()
            );

            _SearchCommand = new Commands.DelegateCommand(PerformSearch);

            _AddCommand = new Commands.DelegateCommand(PerformAddFile);
            _AddReferenceCommand = new Commands.DelegateCommand(PerformAddReference);
            _AddBlueprintsCommand = new Commands.DelegateCommand(PerformAddBlueprints);
            _AddExistingCommand = new Commands.DelegateCommand(PerformAddExistingFile);
            _AddFolderCommand = new Commands.DelegateCommand(PerformAddFolder);
            _AddCollectionCommand = new Commands.DelegateCommand(PerformAddCollection);

            _RemoveReferenceCommand = new Commands.DelegateCommand(PerformRemoveReference);

            _OpenProjectCommand = new Commands.DelegateCommand(PerformOpenProject);
            _NewProjectCommand = new Commands.DelegateCommand(PerformNewProject);
            _CloseProjectCommand = new Commands.DelegateCommand(PerformCloseProject);

            _ViewCodeCommand = new Commands.DelegateCommand(PerformViewCode);

            _RenameCommand = new Commands.DelegateCommand(PerformRename);
            _DeleteCommand = new Commands.DelegateCommand(PerformDelete);
        }

        private ProjectItemViewModel _RootItem;
        public void SetRootItem(ProjectItem root)
        {
            if (root == null)
            {
                _RootItem = null;
                _First.Clear();
                return;
            }

            SetProject(root, this);

            _RootItem = new ProjectItemViewModel(root);

            _First.Clear();
            _First.Add(_RootItem);
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
            if (_RootItem == null)
            {
                return;
            }

            string serialized = Serialization.Convert.ToSerialized(Model);

            // make it human readable
            serialized = System.Xml.Linq.XDocument.Parse(serialized).ToString() + Environment.NewLine;

            string directory = Path.GetDirectoryName(Model.Path);
            string filename = Model.Path;

            Directory.CreateDirectory(directory);

            File.Write(filename, serialized);
        }

        public ProjectItemViewModel GetSelectedFile()
        {
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return null;
            }
            if (selected.Type == ProjectItemType.File)
            {
                return selected;
            }
            return null;
        }

        private void LoadBlueprints()
        {
            bool failed = false;

            LoadBlueprints(_RootItem, failed);
            if (failed)
            {
                Services.MessageBox.ShowMessage("Failed to load one or more blueprints.");
            }
        }

        private void LoadBlueprints(ProjectItemViewModel item, bool failed)
        {
            if (item.Type == ProjectItemType.Blueprints)
            {
                string name;
                Interop.Grid grid;

                Interop.Blueprint.Import(item.Blueprint, out name, out grid);

                if (grid == null)
                {
                    failed = true;
                }
                else
                {
                    item.SetGrid(grid);
                }
            }
            foreach (ProjectItemViewModel child in item.Children)
            {
                LoadBlueprints(child, failed);
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
            if (item == null)
            {
                yield return null;
            }

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

            ProjectItemViewModel parent = (ProjectItemViewModel)child.Parent;
            if (parent.Type == ProjectItemType.Root || parent.Type == ProjectItemType.Collection || parent.Type == ProjectItemType.Blueprints || parent.Type == ProjectItemType.Folder)
            {
                return parent;
            }
            else
            {
                return GetParentFolder(parent);
            }
        }

        public ProjectItemViewModel GetParentBlueprint(ProjectItemViewModel item)
        {
            if (item == null || item.Type == ProjectItemType.Root)
            {
                return null;
            }
            if (item.Type == ProjectItemType.Blueprints)
            {
                return item;
            }
            if (item.Parent == null)
            {
                return null;
            }
            else
            {
                ProjectItemViewModel parent = (ProjectItemViewModel)item.Parent;
                return GetParentBlueprint(parent);
            }
        }

        public ProjectItemViewModel GetSelectedBlueprint()
        {
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return null;
            }
            if (selected.Type == ProjectItemType.Root)
            {
                return null;
            }
            if (selected.Type != ProjectItemType.Blueprints)
            {
                selected = GetParentBlueprint(selected);
            }
            if (selected == null)
            {
                return null;
            }
            else
            {
                return selected;
            }
        }

        public ProjectItemViewModel GetItemByPath(string path)
        {
            if (_RootItem == null)
            {
                return null;
            }
            else
            {
                return GetItemByPath(_RootItem, path);
            }
        }

        public ProjectItemViewModel GetItemByPath(ProjectItemViewModel item, string path)
        {
            if (item.Path == path)
            {
                return item;
            }
            foreach (ProjectItemViewModel child in item.Children)
            {
                ProjectItemViewModel result = GetItemByPath(child, path);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public List<string> GetAssociatedScripts(string path)
        {
            List<string> scripts = new List<string>();

            ProjectItemViewModel item = GetItemByPath(path);
            if (item != null)
            {
                ProjectItemViewModel collection = GetParentCollection(item);
                if (collection != null)
                {
                    List<string> add = new List<string>();
                    scripts.AddRange(CollectScripts(path, collection, add));
                }
            }

            return scripts;
        }

        public Dictionary<string, string> GetImports(string path, bool unsaved = false)
        {
            Dictionary<string, string> imports = new Dictionary<string, string>();

            ProjectItemViewModel item = GetItemByPath(path);
            if (item != null)
            {
                string code = File.Read(item.Path);
                if (unsaved)
                {
                    code = item.Unsaved;
                }

                List<string> lines = new List<string>(System.Text.RegularExpressions.Regex.Split(code, Environment.NewLine));
                foreach (string line in lines)
                {
                    System.Text.RegularExpressions.Regex import = new System.Text.RegularExpressions.Regex(@"^#import\s[a-zA-Z0-9\-_()\[\]+~`'.!@#$%^&]+$");
                    if (import.IsMatch(line))
                    {
                        string name = line.Replace("#import ", String.Empty);

                        ProjectItemViewModel reference = GetReferences().FirstOrDefault(i => i.Name == name);
                        if (reference != null)
                        {
                            if (!imports.ContainsKey(name))
                            {
                                string content = File.Read(reference.Path);

                                imports.Add(name, content);
                            }
                        }
                    }
                }
            }

            return imports;
        }

        public string ImportReferences(string code)
        {
            List<string> imports = new List<string>();

            List<string> lines = new List<string>(System.Text.RegularExpressions.Regex.Split(code, Environment.NewLine));
            foreach (string line in lines)
            {
                System.Text.RegularExpressions.Regex import = new System.Text.RegularExpressions.Regex(@"^#import\s[a-zA-Z0-9\-_()\[\]+~`'.!@#$%^&]+$");
                if (import.IsMatch(line))
                {
                    string name = line.Replace("#import ", String.Empty);
                    if (!imports.Contains(name))
                    {
                        imports.Add(name);
                    }
                }
            }

            foreach (string import in imports)
            {
                ProjectItemViewModel reference = GetReferences().FirstOrDefault(i => i.Name == import);
                if (reference != null)
                {
                    string content = File.Read(reference.Path);
                    code = code.Replace("#import " + import, String.Empty);

                    code += Environment.NewLine + Environment.NewLine + content;
                }
            }

            return code;
        }

        private List<string> CollectScripts(string path, ProjectItemViewModel item, List<string> scripts)
        {
            if (item == null)
            {
                return scripts;
            }

            if (item.Type == ProjectItemType.File && item.Path != path)
            {
                string code = File.Read(item.Path);
                scripts.Add(code);
            }
            foreach (ProjectItemViewModel child in item.Children)
            {
                List<string> empty = new List<string>();
                scripts.AddRange(CollectScripts(path, child, empty));
            }

            return scripts;
        }

        private ProjectItemViewModel GetParentCollection(ProjectItemViewModel item)
        {
            if (item == null || item.Type == ProjectItemType.Root)
            {
                return null;
            }
            if (item.Type == ProjectItemType.Collection)
            {
                return item;
            }
            if (item.Parent == null)
            {
                return null;
            }
            else
            {
                ProjectItemViewModel parent = (ProjectItemViewModel)item.Parent;
                return GetParentCollection(parent);
            }
        }

        private List<ProjectItemViewModel> GetReferences()
        {
            List<ProjectItemViewModel> references = new List<ProjectItemViewModel>();
            if (_RootItem != null)
            {
                foreach(ProjectItemViewModel item in _RootItem.Children)
                {
                    if (item.Type == ProjectItemType.References)
                    {
                        foreach (ProjectItemViewModel reference in item.Children)
                        {
                            if (reference.Type == ProjectItemType.Reference)
                            {
                                references.Add(reference);
                            }
                        }
                    }
                }
            }
            return references;
        }

        #region Commands

        #region New Project Command

        private readonly ICommand _NewProjectCommand;
        public ICommand NewProjectCommand
        {
            get
            {
                return _NewProjectCommand;
            }
        }

        public void PerformNewProject()
        {
            if (!IsClosed)
            {
                return;
            }

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string initial = Path.Combine(documents, "SEWorkbench");

            bool success = false;
            try
            {
                // TODO Move all IO to a separate class
                Directory.CreateDirectory(initial);

                Views.NewProjectDialog view = new Views.NewProjectDialog()
                {
                    ProjectLocation = initial,
                };

                Nullable<bool> result = view.ShowDialog();
                if (result != null && result.Value == true)
                {
                    string name = view.ProjectName;
                    string filename = String.Format("{0}.seproj", name);
                    string fullpath = Path.Combine(initial, name, filename);

                    Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

                    ProjectViewModel project = new ViewModels.ProjectViewModel(null);
                    ProjectItem root = new ProjectItem()
                    {
                        Name = name,
                        Type = Models.ProjectItemType.Root,
                        Path = fullpath,
                        Project = project,
                    };

                    root.Children.Add(new ProjectItem() { Name = "References", Type = ProjectItemType.References });

                    SetRootItem(root);

                    success = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ShowError("Unable to create new project", ex);
                return;
            }
            finally
            {
                if (success)
                {
                    CreateNewProjectTemplate();
                    SaveProject();

                    RaiseProjectCreated();
                }
            }
        }

        private void CreateNewProjectTemplate()
        {
            if (_RootItem == null)
            {
                return;
            }

            string demofolder = "Library";
            string rootpath = Path.GetDirectoryName(_RootItem.Path);
            string folderpath = Path.Combine(rootpath, demofolder);
            string filepath = Path.Combine(folderpath, String.Format("NewScript.csx"));

            try
            {
                Directory.CreateDirectory(folderpath);
                ProjectItem folder = new ProjectItem()
                {
                    Name = demofolder,
                    Path = folderpath,
                    Type = ProjectItemType.Folder,
                    Project = this,
                };

                File.Write(filepath, Services.NewFile.Contents);
                ProjectItem file = new ProjectItem()
                {
                    Name = Path.GetFileNameWithoutExtension(filepath),
                    Path = filepath,
                    Type = ProjectItemType.File,
                    Project = this,
                };

                ProjectItemViewModel newfile = null;

                _RootItem.AddChild(folder);
                if (_RootItem.Children.Count > 0)
                {
                    foreach(ProjectItemViewModel item in _RootItem.Children)
                    {
                        if (item.Type == ProjectItemType.Folder)
                        {
                            newfile = item.AddChild(file);
                            if (item.Children.Count > 0)
                            {
                                item.Children[0].IsSelected = true;
                            }
                            item.IsExpanded = true;

                            break;
                        }
                    }
                }

                if (newfile != null)
                {
                    RaiseFileCreated(newfile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.ShowError("Unable to create new project template", ex);
            }
        }

        #endregion

        #region Remove Reference Command

        private readonly ICommand _RemoveReferenceCommand;
        public ICommand RemoveReferenceCommand
        {
            get
            {
                return _RemoveReferenceCommand;
            }
        }

        public void PerformRemoveReference()
        {
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return;
            }
            if (selected.Type != ProjectItemType.Reference)
            {
                return;
            }

            System.Windows.MessageBoxResult result =
                Services.MessageBox.ShowQuestion(
                    String.Format("Are you sure you want to remove this reference ({0})?", selected.Name)
                );

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                foreach (ProjectItemViewModel item in _RootItem.Children)
                {
                    if (item.Type == ProjectItemType.References)
                    {
                        foreach (ProjectItemViewModel reference in item.Children)
                        {
                            if (reference.Path == selected.Path)
                            {
                                reference.Remove();
                                break;
                            }
                        }
                    }
                }
                SaveProject();
            }
        }

        #endregion

        #region Add Reference Command

        private readonly ICommand _AddReferenceCommand;
        public ICommand AddReferenceCommand
        {
            get
            {
                return _AddReferenceCommand;
            }
        }

        public void PerformAddReference()
        {
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return;
            }
            if (selected.Type != ProjectItemType.References)
            {
                return;
            }

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string initial = Path.Combine(documents, "SEWorkbench");

            Views.AddReferenceView view = new Views.AddReferenceView()
            {
                Initial = initial
            };

            Nullable<bool> result = view.ShowDialog();
            if (result != null && result.Value == true)
            {
                string path = view.Filename;
                string name = Path.GetFileNameWithoutExtension(path);

                ProjectItemViewModel existing = GetReferences().FirstOrDefault(i => i.Name == name);
                if (existing != null)
                {
                    Services.MessageBox.ShowMessage(String.Format("A reference with this name already exists ({0}).", name));
                    return;
                }

                try
                {
                    ProjectItemViewModel reference = null;

                    ProjectItem item = new ProjectItem()
                    {
                        Name = name,
                        Path = path,
                        Type = ProjectItemType.Reference,
                        Project = this
                    };

                    reference = selected.AddChild(item);
                    if (reference != null)
                    {
                        RaiseReferenceAdded(reference);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.ShowError("Unable to create new file", ex);
                    return;
                }
                SaveProject();
            }
        }

        #endregion

        #region Open Project Command

        private readonly ICommand _OpenProjectCommand;
        public ICommand OpenProjectCommand
        {
            get
            {
                return _OpenProjectCommand;
            }
        }

        public void PerformOpenProject()
        {
            if (!IsClosed)
            {
                return;
            }

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string initial = Path.Combine(documents, "SEWorkbench");

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
                try
                {
                    string fullpath = dialog.FileName;
                    string serialized = File.Read(fullpath);

                    Project project = (Project)Serialization.Convert.ToObject(serialized);

                    OpenState = project.OpenState;
                    SetRootItem(project.RootItem);

                    LoadBlueprints();

                    if (!ReferencesExists(_RootItem))
                    {
                        _RootItem.AddChild(new ProjectItem() { Name = "References", Type = ProjectItemType.References });
                    }

                    _RootItem.IsExpanded = true;

                    SaveProject();

                    RaiseProjectOpened();
                }
                catch (Exception ex)
                {
                    SetRootItem(null);
                    MessageBox.ShowError("Unable to open project", ex);
                }

                foreach (ProjectItemViewModel child in First)
                {
                    child.VerifyPath();
                }
            }
        }

        public List<BackupItem> GetBackups()
        {
            List<BackupItem> result = new List<BackupItem>();
            foreach (ProjectItemViewModel child in First)
            {
                result.AddRange(GetBackups(child));
            }
            return result;
        }

        private List<BackupItem> GetBackups(ProjectItemViewModel item)
        {
            BackupManager manager = new BackupManager(this.Model.Path);
            
            List<BackupItem> result = new List<BackupItem>();

            if (item.Type == ProjectItemType.File)
            {
                BackupItem backup = manager.GetBackup(item.Path);
                if (backup != null)
                {
                    result.Add(backup);
                }
            }

            foreach(ProjectItemViewModel child in item.Children)
            {
                result.AddRange(GetBackups(child));
            }

            return result;
        }

        private bool ReferencesExists(ProjectItemViewModel root)
        {
            foreach(ProjectItemViewModel child in root.Children)
            {
                if (child.Type == ProjectItemType.References)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Close Project Command

        private readonly ICommand _CloseProjectCommand;
        public ICommand CloseProjectCommand
        {
            get
            {
                return _CloseProjectCommand;
            }
        }

        public void PerformCloseProject()
        {
            OpenState = false;
            SaveProject();

            if (_RootItem != null)
            {
                SetRootItem(null);

                RaiseProjectClosed();
            }
        }

        #endregion

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
            if (_MatchingItemEnumerator == null)
            {
                return;
            }

            var item = _MatchingItemEnumerator.Current;
            if (item == null)
            {
                return;
            }

            if (item.Parent != null)
            {
                ProjectItemViewModel parent = (ProjectItemViewModel)item.Parent;
                parent.IsExpanded = true;
            }

            // TODO switch search to filter (hide unmatched items), probably handled by the UI instead

            item.IsSelected = true;
        }

        private void VerifyMatchingItemEnumerator()
        {
            if (First == null || First.Count < 1)
            {
                return;
            }

            var matches = FindMatches(_SearchText, _RootItem);

            _MatchingItemEnumerator = matches.GetEnumerator();
            if (!_MatchingItemEnumerator.MoveNext())
            {
                // none found, do nothing for now
            }
        }

        private IEnumerable<ProjectItemViewModel> FindMatches(string text, ProjectItemViewModel item)
        {
            if (item == null)
            {
                yield return null;
            }
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
            if (selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Collection && selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }
            if (selected == null || selected.Type == ProjectItemType.Root)
            {
                return;
            }

            Views.NewItemDialog view = new Views.NewItemDialog();
            Nullable<bool> result = view.ShowDialog();
            if (result != null && result.Value == true)
            {
                string fullpath = Path.Combine(selected.Path, String.Format("{0}.csx", view.ItemName));
                try
                {
                    string starter = Services.NewFile.Contents;

                    File.Write(fullpath, starter);

                    ProjectItemViewModel newfile = null;

                    string name = Path.GetFileNameWithoutExtension(fullpath);
                    ProjectItem item = new ProjectItem()
                    {
                        Name = name,
                        Path = fullpath,
                        Type = ProjectItemType.File,
                        Project = this
                    };
                    newfile = selected.AddChild(item);
                    selected.IsExpanded = true;

                    if (newfile != null)
                    {
                        RaiseFileCreated(newfile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.ShowError("Unable to create new file", ex);
                    return;
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
            if (selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Collection && selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }
            if (selected == null || selected.Type == ProjectItemType.Root)
            {
                return;
            }

            string initial = selected.Path;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".csx",
                Filter = "Script File (.csx)|*.csx",
                InitialDirectory = initial,
                Multiselect = true,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result != null && result.Value == true)
            {
                foreach (string source in dialog.FileNames)
                {
                    string destination = Path.Combine(initial, Path.GetFileName(source));
                    if (File.Exists(destination))
                    {
                        string message = String.Format(
                            "A file with the name \"{0}\" already exists. Do you want to overwrite it?",
                            Path.GetFileName(source)
                        );

                        try
                        {
                            if (Path.GetDirectoryName(source) != Path.GetDirectoryName(destination))
                            {
                                System.Windows.MessageBoxResult overwrite = MessageBox.ShowQuestion(message);
                                if (overwrite == System.Windows.MessageBoxResult.No)
                                {
                                    continue;
                                }
                                if (overwrite == System.Windows.MessageBoxResult.Cancel)
                                {
                                    break;
                                }
                                File.Copy(source, destination);
                            }

                            string code = File.Read(destination);

                            string name = Path.GetFileNameWithoutExtension(destination);
                            ProjectItem item = new ProjectItem()
                            {
                                Name = name,
                                Path = destination,
                                Type = ProjectItemType.File,
                                Project = this
                            };
                            selected.AddChild(item);
                            selected.IsExpanded = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.ShowError("Unable to create new file", ex);
                            return;
                        }
                    }
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
                return;
            }
            if (selected.Type != ProjectItemType.Root && selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Collection && selected.Type != ProjectItemType.Folder)
            {
                selected = GetParentFolder(selected);
            }
            if (selected == null)
            {
                return;
            }

            Views.NewItemDialog view = new Views.NewItemDialog();
            Nullable<bool> result = view.ShowDialog();
            if (result != null && result.Value == true)
            {
                string name = view.ItemName;

                string fullpath = Path.Combine(selected.Path, name);
                if (selected.Type == ProjectItemType.Root)
                {
                    fullpath = Path.Combine(Path.GetDirectoryName(selected.Path), name);
                }

                try
                {
                    Directory.CreateDirectory(fullpath);
                    ProjectItem item = new ProjectItem()
                    {
                        Name = name,
                        Path = fullpath,
                        Type = ProjectItemType.Folder,
                        Project = this,
                    };
                    selected.AddChild(item);
                    selected.IsExpanded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.ShowError("Unable to create new directory", ex);
                    return;
                }
                SaveProject();
            }
        }

        #endregion

        #region Add Collection Command

        private readonly ICommand _AddCollectionCommand;
        public ICommand AddCollectionCommand
        {
            get
            {
                return _AddCollectionCommand;
            }
        }

        public void PerformAddCollection()
        {
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return;
            }
            if (selected.Type != ProjectItemType.Root && selected.Type != ProjectItemType.Blueprints)
            {
                return;
            }

            Views.NewItemDialog view = new Views.NewItemDialog();
            Nullable<bool> result = view.ShowDialog();
            if (result != null && result.Value == true)
            {
                string name = view.ItemName;

                string fullpath = Path.Combine(selected.Path, name);
                if (selected.Type == ProjectItemType.Root)
                {
                    fullpath = Path.Combine(Path.GetDirectoryName(selected.Path), name);
                }

                try
                {
                    Directory.CreateDirectory(fullpath);
                    ProjectItem item = new ProjectItem()
                    {
                        Name = name,
                        Path = fullpath,
                        Type = ProjectItemType.Collection,
                        Project = this,
                    };
                    selected.AddChild(item);
                    selected.IsExpanded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.ShowError("Unable to create new collection", ex);
                    return;
                }
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
            string blueprints = Path.Combine(appdata, "SpaceEngineers", "Blueprints");

            // TODO implement my own open file dialog to avoid API
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".sbc",
                Filter = "Blueprints File (.sbc)|*.sbc",
                Multiselect = false,
                InitialDirectory = blueprints,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result != null && result.Value == true)
            {
                string fullpath = dialog.FileName;

                string name;
                Interop.Grid grid;

                Interop.Blueprint.Import(fullpath, out name, out grid);
                if (grid != null)
                {
                    string safename = CreateSafeName(name);
                    string savepath = Path.Combine(Path.GetDirectoryName(rootitem.Path), safename);
                    try
                    {
                        Directory.CreateDirectory(savepath);
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ShowError("Unable to create add blueprint", ex);
                        return;
                    }
                    SaveProject();
                }
            }
        }

        private string CreateSafeName(string text)
        {
            string[] parts = text.Split(Services.Strings.InvalidFilenameChars, StringSplitOptions.RemoveEmptyEntries);

            return String.Join("", parts);
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
            RaiseFileRequested();
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
            ProjectItemViewModel selected = SelectedItem;
            if (selected == null)
            {
                return;
            }
            if (selected.Type != ProjectItemType.Collection && selected.Type != ProjectItemType.Folder && selected.Type != ProjectItemType.File)
            {
                return;
            }

            Views.NewItemDialog view = new Views.NewItemDialog();
            Nullable<bool> result = view.ShowDialog();
            if (result != null && result.Value == true)
            {
                string source = selected.Path;
                string destination = String.Empty;
                if (selected.Type == ProjectItemType.File)
                {
                    string filename = String.Format("{0}.csx", view.ItemName);
                    destination = Path.Combine(Path.GetDirectoryName(source), filename);
                    if (File.Exists(destination))
                    {
                        MessageBox.ShowMessage(String.Format("A file with the name \"{0}\" already exists.", destination));
                        return;
                    }
                    try
                    {
                        File.Move(source, destination);

                        selected.Name = view.ItemName;
                        selected.Path = destination;

                        SaveProject();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ShowError("Unable to rename file", ex);
                        return;
                    }
                }

                if (selected.Type == ProjectItemType.Collection || selected.Type == ProjectItemType.Folder)
                {
                    destination = Path.Combine(Directory.GetParent(source).FullName, view.ItemName);
                    if (Directory.Exists(destination))
                    {
                        MessageBox.ShowMessage(String.Format("A folder with the name \"{0}\" already exists.", destination));
                        return;
                    }
                    try
                    {
                        Directory.Move(source, destination);

                        selected.Name = view.ItemName;
                        selected.Path = destination;

                        selected.UpdatePath(source, destination);

                        SaveProject();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ShowError("Unable to rename directory", ex);
                        return;
                    }
                }
            }
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
            if (selected == null)
            {
                return;
            }
            if (selected.Type == ProjectItemType.Reference)
            {
                PerformRemoveReference();
                return;
            }
            if (selected.Type != ProjectItemType.Blueprints && selected.Type != ProjectItemType.Folder && selected.Type != ProjectItemType.File)
            {
                return;
            }

            string message = String.Format("'{0}' will be deleted permanantly?", selected.Name);

            var result = MessageBox.ShowQuestion(message);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                bool success = false;
                try
                {
                    switch (selected.Type)
                    {
                        case ProjectItemType.Blueprints:
                        case ProjectItemType.Folder:
                            Directory.Delete(selected.Path, true);
                            break;

                        case ProjectItemType.File:
                            File.Delete(selected.Path);
                            break;
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    MessageBox.ShowError("Unable to delete the selected item", ex);
                }
                finally
                {
                    if (success)
                    {
                        selected.Remove();
                        SaveProject();

                        RaiseFileDeleted(selected);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
