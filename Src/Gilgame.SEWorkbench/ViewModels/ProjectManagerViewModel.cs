using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectManagerViewModel : BaseViewModel
    {
        private Regex _LineColRegex = new Regex(@"\(([0-9]+),([0-9]+)\)");
        private Regex _ErrorRegex = new Regex(@"^(.*?)\(([0-9]+),([0-9]+)\)\s:\serror\s(.*?):\s(.*?)$");

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

        private ObservableCollection<MenuItemViewModel> _WindowMenuItems = new ObservableCollection<MenuItemViewModel>();
        public ObservableCollection<MenuItemViewModel> WindowMenuItems
        {
            get
            {
                return _WindowMenuItems;
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

        private BlueprintViewModel _Blueprint;
        public BlueprintViewModel Blueprint
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

        private EditorViewModel _Editor;
        public EditorViewModel Editor
        {
            get
            {
                return _Editor;
            }
            private set
            {
                _Editor = value;
                OnPropertyChanged("Editor");
            }
        }

        private OutputViewModel _Output;
        public OutputViewModel Output
        {
            get
            {
                return _Output;
            }
            private set
            {
                _Output = value;
                OnPropertyChanged("Output");
            }
        }

        public bool IsModified
        {
            get
            {
                foreach(PageViewModel page in Editor.Items)
                {
                    if (page.IsModified)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public ProjectManagerViewModel(BaseViewModel parent) : base(parent)
        {
            // create project object first
            Project = new ProjectViewModel(this);
            Project.FileRequested += Project_FileRequested;
            Project.SelectionChanged += Project_SelectionChanged;
            Project.FileCreated += Project_FileCreated;
            Project.FileDeleted += Project_FileDeleted;

            Blueprint = new BlueprintViewModel(this);
            Blueprint.InsertRequested += Blueprint_InsertRequested;

            Editor = new EditorViewModel(this);
            Editor.Items.CollectionChanged += Editor_CollectionChanged;

            Output = new OutputViewModel(this);

            _NewProjectCommand = new Commands.NewProjectCommand(this);
            _OpenProjectCommand = new Commands.OpenProjectCommand(this);
            _CloseProjectCommand = new Commands.CloseProjectCommand(this);

            _SaveFileCommand = new Commands.SaveFileCommand(this);
            _SaveAllCommand = new Commands.SaveAllCommand(this);

            _RunScriptCommand = new Commands.RunScriptCommand(this);
            _OpenSelectedCommand = new Commands.OpenSelectedCommand(this);

            _CloseFileCommand = new Commands.CloseFileCommand(this);
            _CloseAllCommand = new Commands.CloseAllCommand(this);

            _UndoCommand = new Commands.UndoCommand(this);
            _RedoCommand = new Commands.RedoCommand(this);
            _CutCommand = new Commands.CutCommand(this);
            _CopyCommand = new Commands.CopyCommand(this);
            _PasteCommand = new Commands.PasteCommand(this);
            _DeleteCommand = new Commands.DeleteCommand(this);
            _SelectAllCommand = new Commands.SelectAllCommand(this);

            _CloseViewCommand = new Commands.CloseViewCommand(this);
        }

        public event EventHandler CloseViewRequested;
        private void RaiseCloseViewRequested()
        {
            if (CloseViewRequested != null)
            {
                CloseViewRequested(this, EventArgs.Empty);
            }
        }

        public event EventHandler ScriptRunning;
        private void RaiseScriptRunning()
        {
            if (ScriptRunning != null)
            {
                ScriptRunning(this, EventArgs.Empty);
            }
        }

        public bool HandleClosing()
        {
            if (IsModified)
            {
                MessageBoxResult result = Services.MessageBox.ShowQuestion("One or more files have been modified. Would you like to save them now?");
                if (result == MessageBoxResult.Yes)
                {
                    PerformSaveAll();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }

        private void Project_SelectionChanged(object sender, EventArgs e)
        {
            ProjectItemViewModel item = Project.GetSelectedBlueprint();
            if (item != null)
            {
                Blueprint.SetBlueprint(item.Grid);
            }
        }

        private void Project_FileCreated(object sender, FileEventArgs e)
        {
            ProjectItemViewModel item = Project.GetItemByPath(e.Path);
            PerformOpenItem(item);
        }

        private void Project_FileDeleted(object sender, FileEventArgs e)
        {
            Editor.Items.Remove(Editor.Items.Where(i => i.Filename == e.Path).Single());
        }

        private void Project_FileRequested(object sender, EventArgs e)
        {
            PerformOpenSelected();
        }

        private void Page_CloseFileRequested(object sender, FileEventArgs e)
        {
            PerformCloseFile(e.Path);
        }

        private void Blueprint_InsertRequested(object sender, InsertEventArgs e)
        {
            Editor.InsertText(e.Text);
        }

        private void Editor_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (object o in e.NewItems)
                {
                    if (o is PageViewModel)
                    {
                        PageViewModel page = (PageViewModel)o;

                        WindowMenuItems.Add(new MenuItemViewModel(this, page.Name, page.SelectFileCommand) { Identifier = page.Identifier });
                    }
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (object o in e.OldItems)
                {
                    if (o is PageViewModel)
                    {
                        PageViewModel page = (PageViewModel)o;
                        foreach (MenuItemViewModel item in WindowMenuItems)
                        {
                            if (item.Identifier == page.Identifier)
                            {
                                WindowMenuItems.Remove(item);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void FindError(OutputItemViewModel item)
        {
            if (item != null)
            {
                PageViewModel page = Editor.Items.Where(i => i.Filename == item.Filename).Single();
                if (page != null)
                {
                    DocumentLine line = page.Content.Document.GetLineByNumber(item.Line);
                    page.Content.Select(line.Offset, line.Length);
                    page.Content.TextArea.Caret.BringCaretToView();
                }
            }
        }

        #region Commands

        #region Open Selected Command

        private readonly ICommand _OpenSelectedCommand;
        public ICommand OpenSelectedCommand
        {
            get
            {
                return _OpenSelectedCommand;
            }
        }

        public void PerformOpenSelected()
        {
            ProjectItemViewModel item = _Project.GetSelectedFile();
            PerformOpenItem(item);
        }

        public void PerformOpenItem(ProjectItemViewModel item)
        {
            if (item != null)
            {
                foreach (PageViewModel page in Editor.Items)
                {
                    if (page.Filename == item.Path)
                    {
                        page.IsSelected = true;
                        return;
                    }
                }

                PageViewModel newpage = new PageViewModel(this, item.Name, item.Path);
                newpage.FileCloseRequested += Page_CloseFileRequested;

                Editor.Items.Add(newpage);
            }
        }

        #endregion

        #region Run Script Command

        private readonly ICommand _RunScriptCommand;
        public ICommand RunScriptCommand
        {
            get
            {
                return _RunScriptCommand;
            }
        }

        public void PerformRunScript()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                Interop.InGameScript script = new Interop.InGameScript(page.Content.Text);

                string filename = page.Filename;

                Output.Clear();
                RaiseScriptRunning();
                
                if (script.CompileErrors.Count > 0)
                {
                    foreach(string error in script.CompileErrors)
                    {
                        string message = error;

                        Match full = _ErrorRegex.Match(error);
                        if (full.Groups.Count > 1)
                        {
                            int line = Convert.ToInt32(full.Groups[2].Value) - 9;
                            int col = Convert.ToInt32(full.Groups[3].Value);

                            string errno = full.Groups[4].Value;

                            string errmsg = full.Groups[5].Value;

                            Models.OutputItem item = new Models.OutputItem()
                            {
                                Line = line,
                                Column = col,
                                Error = errno,
                                Message = errmsg,
                                Filename = filename
                            };
                            Output.AddItem(item);
                        }
                        else
                        {
                            Match match = _LineColRegex.Match(error);
                            if (match.Groups.Count > 1)
                            {
                                int line = Convert.ToInt32(match.Groups[2].Value) - 9;
                                int col = Convert.ToInt32(match.Groups[3].Value);

                                message = message.Replace(match.Groups[0].Value, "");

                                Output.AddItem(new Models.OutputItem() { Line = line, Column = col, Message = message, Filename = filename });
                            }
                            else
                            {
                                Output.AddItem(new Models.OutputItem() { Message = message, Filename = filename });
                            }
                        }
                    }
                }
                else
                {
                    Services.MessageBox.ShowMessage("The program compiled without any errors.");
                }
            }
        }

        #endregion

        #region Save File Command

        private readonly ICommand _SaveFileCommand;
        public ICommand SaveFileCommand
        {
            get
            {
                return _SaveFileCommand;
            }
        }

        public void PerformSaveFile()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                page.Save();
            }
        }

        #endregion

        #region Save All Command

        private readonly ICommand _SaveAllCommand;
        public ICommand SaveAllCommand
        {
            get
            {
                return _SaveAllCommand;
            }
        }

        public void PerformSaveAll()
        {
            foreach (PageViewModel page in Editor.Items)
            {
                page.Save();
            }
        }

        #endregion

        #region Close File Command

        private readonly ICommand _CloseFileCommand;
        public ICommand CloseFileCommand
        {
            get
            {
                return _CloseFileCommand;
            }
        }

        public void PerformCloseFile()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                if (page.IsModified)
                {
                    MessageBoxResult result = Services.MessageBox.ShowQuestion("this file has been modified. Would you like to save it now?");
                    if (result == MessageBoxResult.Yes)
                    {
                        page.Save();
                    }
                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                page.FileCloseRequested -= Page_CloseFileRequested;

                Editor.Items.Remove(page);
            }
        }

        public void PerformCloseFile(string path)
        {
            PageViewModel page = Editor.Items.Where(i => i.Filename == path).Single();
            if (page != null)
            {
                if (page.IsModified)
                {
                    MessageBoxResult result = Services.MessageBox.ShowQuestion("this file has been modified. Would you like to save it now?");
                    if (result == MessageBoxResult.Yes)
                    {
                        page.Save();
                    }
                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                page.FileCloseRequested -= Page_CloseFileRequested;

                Editor.Items.Remove(page);
            }
        }

        #endregion

        #region Close all Command

        private readonly ICommand _CloseAllCommand;
        public ICommand CloseAllCommand
        {
            get
            {
                return _CloseAllCommand;
            }
        }

        public void PerformCloseAll()
        {
            if (IsModified)
            {
                MessageBoxResult result = Services.MessageBox.ShowQuestion("One or more files have been modified. Would you like to save them now?");
                if (result == MessageBoxResult.Yes)
                {
                    PerformSaveAll();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            Editor.Items.Clear();
        }

        #endregion

        #region Undo Command

        private readonly ICommand _UndoCommand;
        public ICommand UndoCommand
        {
            get
            {
                return _UndoCommand;
            }
        }

        public void PerformUndo()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                page.Content.Undo();
            }
        }

        #endregion

        #region Redo Command

        private readonly ICommand _RedoCommand;
        public ICommand RedoCommand
        {
            get
            {
                return _RedoCommand;
            }
        }

        public void PerformRedo()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                page.Content.Redo();
            }
        }

        #endregion

        #region Cut Command

        private readonly ICommand _CutCommand;
        public ICommand CutCommand
        {
            get
            {
                return _CutCommand;
            }
        }

        public void PerformCut()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                page.Content.Cut();
            }
        }

        #endregion

        #region Copy Command

        private readonly ICommand _CopyCommand;
        public ICommand CopyCommand
        {
            get
            {
                return _CopyCommand;
            }
        }

        public void PerformCopy()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                page.Content.Copy();
            }
        }

        #endregion

        #region Paste Command

        private readonly ICommand _PasteCommand;
        public ICommand PasteCommand
        {
            get
            {
                return _PasteCommand;
            }
        }

        public void PerformPaste()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                page.Content.Paste();
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
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                // TODO implement
            }
        }

        #endregion

        #region Select All Command

        private readonly ICommand _SelectAllCommand;
        public ICommand SelectAllCommand
        {
            get
            {
                return _SelectAllCommand;
            }
        }

        public void PerformSelectAll()
        {
            PageViewModel page = Editor.SelectedItem;
            if (page != null)
            {
                page.Content.SelectAll();
            }
        }

        #endregion

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
            if (IsModified)
            {
                MessageBoxResult result = Services.MessageBox.ShowQuestion("One or more files have been modified. Would you like to save them now?");
                if (result == MessageBoxResult.Yes)
                {
                    PerformSaveAll();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            Editor.Items.Clear();
            Project.PerformNewProject();
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
            if (IsModified)
            {
                MessageBoxResult result = Services.MessageBox.ShowQuestion("One or more files have been modified. Would you like to save them now?");
                if (result == MessageBoxResult.Yes)
                {
                    PerformSaveAll();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            Editor.Items.Clear();
            Project.PerformOpenProject();
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
            if (IsModified)
            {
                MessageBoxResult result = Services.MessageBox.ShowQuestion("One or more files have been modified. Would you like to save them now?");
                if (result == MessageBoxResult.Yes)
                {
                    PerformSaveAll();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            Project.PerformCloseProject();
            Editor.Items.Clear();
        }

        #endregion

        #region Close View Command

        private readonly ICommand _CloseViewCommand;
        public ICommand CloseViewCommand
        {
            get
            {
                return _CloseViewCommand;
            }
        }

        public void PerformCloseView()
        {
            RaiseCloseViewRequested();
        }

        #endregion

        #endregion
    }
}
