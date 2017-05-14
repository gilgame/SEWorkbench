﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Input;

using Gilgame.SEWorkbench.Services.IO;

using ICSharpCode.AvalonEdit.Document;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class ProjectManagerViewModel : BaseViewModel
    {
        private Regex _LineColRegex = new Regex(@"\(([0-9]+),([0-9]+)\)");
        private Regex _ErrorRegex = new Regex(@"^(.*?)\(([0-9]+),([0-9]+)\)\s:\serror\s(.*?):\s(.*?)$");

        private bool _StopBackupTimer = false;

        private Timer _BackupTimer;

        private const string _ProjectTitlePrefix = "Space Engineers Workbench";

        private bool _Verifying = false;
        public bool IsVerifying
        {
            get
            {
                return _Verifying;
            }
        }

        private string _ProjectTitle = _ProjectTitlePrefix;
        public string ProjectTitle
        {
            get
            {
                return _ProjectTitle;
            }
            set
            {
                _ProjectTitle = value;
                RaisePropertyChanged("ProjectTitle");
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
                RaisePropertyChanged("Project");
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
                RaisePropertyChanged("Blueprint");
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
                RaisePropertyChanged("Editor");
            }
        }

        private ClassViewModel _Classes;
        public ClassViewModel Classes
        {
            get
            {
                return _Classes;
            }
            private set
            {
                _Classes = value;
                RaisePropertyChanged("Classes");
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
                RaisePropertyChanged("Output");
            }
        }

        private BackupViewModel _Backup;
        public BackupViewModel Backup
        {
            get
            {
                return _Backup;
            }
            set
            {
                _Backup = value;
                RaisePropertyChanged("Backup");
            }
        }

        private FindReplaceViewModel _FindReplace;
        public FindReplaceViewModel FindReplace
        {
            get
            {
                return _FindReplace;
            }
            set
            {
                _FindReplace = value;
                RaisePropertyChanged("FindReplace");
            }
        }

        private Config.ConfigViewModel _Config;
        public Config.ConfigViewModel Config
        {
            get
            {
                return _Config;
            }
            set
            {
                _Config = value;
                RaisePropertyChanged("Config");
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

        public event EventHandler BackupsFound;
        private void RaiseBackupsFound()
        {
            var handler = BackupsFound;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public ProjectManagerViewModel(BaseViewModel parent) : base(parent)
        {
            Config = new Config.ConfigViewModel(this);

            Project = new ProjectViewModel(this);
            Project.FileRequested += Project_FileRequested;
            Project.SelectionChanged += Project_SelectionChanged;
            Project.FileCreated += Project_FileCreated;
            Project.ReferenceAdded += Project_ReferenceAdded;
            Project.FileDeleted += Project_FileDeleted;
            Project.ProjectCreated += Project_ProjectCreated;
            Project.ProjectOpened += Project_ProjectOpened;
            Project.ProjectClosed += Project_ProjectClosed;

            Blueprint = new BlueprintViewModel(this);
            Blueprint.InsertRequested += Blueprint_InsertRequested;

            Editor = new EditorViewModel(this);
            Editor.FileChanged += Editor_FileChanged;
            Editor.Items.CollectionChanged += Editor_CollectionChanged;

            Classes = new ClassViewModel(this);
            BuildClasses();

            Output = new OutputViewModel(this);

            Backup = new BackupViewModel(this);
            Backup.RestoreRequested += Backup_RestoreRequested;

            FindReplace = new FindReplaceViewModel(this);

            _NewProjectCommand = new Commands.DelegateCommand(PerformNewProject);
            _OpenProjectCommand = new Commands.DelegateCommand(PerformOpenProject);
            _CloseProjectCommand = new Commands.DelegateCommand(PerformCloseProject);

            _SaveFileCommand = new Commands.DelegateCommand(PerformSaveFile);
            _SaveAllCommand = new Commands.DelegateCommand(PerformSaveAll);

            _RunScriptCommand = new Commands.DelegateCommand(PerformRunScript);
            _OpenSelectedCommand = new Commands.DelegateCommand(PerformOpenSelected);

            _CloseFileCommand = new Commands.DelegateCommand(PerformCloseFile);
            _CloseAllCommand = new Commands.DelegateCommand(PerformCloseAll);

            _UndoCommand = new Commands.DelegateCommand(PerformUndo);
            _RedoCommand = new Commands.DelegateCommand(PerformRedo);
            _CutCommand = new Commands.DelegateCommand(PerformCut);
            _CopyCommand = new Commands.DelegateCommand(PerformCopy);
            _PasteCommand = new Commands.DelegateCommand(PerformPaste);
            _DeleteCommand = new Commands.DelegateCommand(PerformDelete);
            _SelectAllCommand = new Commands.DelegateCommand(PerformSelectAll);

            _CloseViewCommand = new Commands.DelegateCommand(PerformCloseView);
        }

        private void BackupTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_StopBackupTimer)
            {
                if (Project != null)
                {
                    Services.BackupManager backup = new Services.BackupManager(Project.Model.Path);
                    foreach (PageViewModel page in Editor.Items)
                    {
                        if (page.IsModified)
                        {
                            backup.BackupFile(page.Filename, page.Text);
                        }
                    }
                }

                StartBackupTimer();
            }
        }

        private void StartBackupTimer()
        {
            _StopBackupTimer = false;

            _BackupTimer = new Timer()
            {
                Interval = Configuration.Backups.Interval * 60000,
                AutoReset = false,
            };
            _BackupTimer.Elapsed += BackupTimer_Elapsed;
            _BackupTimer.Start();
        }

        private void StopBackupTimer()
        {
            _StopBackupTimer = true;

            if (_BackupTimer != null)
            {
                _BackupTimer.Stop();
            }
        }

        private void BuildClasses()
        {
            Classes.AddNamespaces(Interop.Decompiler.Classes);
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

        public void VerifyFiles()
        {
            if (!IsVerifying)
            {
                BeginVerify();
                foreach (PageViewModel page in _Editor.Items)
                {
                    if (!page.IgnoreUpdates)
                    {
                        ProjectItemViewModel item = _Project.GetItemByPath(page.Filename);

                        if (File.LastWriteTime(page.Filename) > page.LastSaved)
                        {
                            string message = String.Format("{0} has been modified outside SE Workbench. Do you want to reload it?", page.ProjectItem.Name);

                            MessageBoxResult result = Services.MessageBox.ShowQuestion(message);
                            if (result == MessageBoxResult.Yes)
                            {
                                page.UpdateContent();
                            }
                            else
                            {
                                page.IgnoreUpdates = true;
                            }
                        }
                    }
                }
                EndVerify();
            }
            VerifyPaths();
        }

        private void BeginVerify()
        {
            _Verifying = true;
        }

        private void EndVerify()
        {
            _Verifying = false;
        }

        public void VerifyPaths()
        {
            foreach (ProjectItemViewModel child in Project.First)
            {
                child.VerifyPath();
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

            Editor.Items.Clear();

            Project.SaveProject();

            StopBackupTimer();

            return true;
        }

        private void Project_SelectionChanged(object sender, EventArgs e)
        {
            ProjectItemViewModel item = Project.GetSelectedBlueprint();
            if (item != null)
            {
                Blueprint.SetBlueprint(item.Grid);
            }
            else
            {
                Blueprint.SetBlueprint(null);
            }
        }

        private void Project_FileCreated(object sender, FileEventArgs e)
        {
            ProjectItemViewModel item = Project.GetItemByPath(e.Path);
            PerformOpenItem(item);
        }

        private void Project_ReferenceAdded(object sender, FileEventArgs e)
        {
            ProjectItemViewModel item = Project.GetItemByPath(e.Path);

        }

        private void Project_FileDeleted(object sender, FileEventArgs e)
        {
            PageViewModel page = null;
            try { page = Editor.Items.Where(i => i.Filename == e.Path).Single(); } catch { /* if we get here, it wasn't open */ }
            if (page != null)
            {
                Editor.Items.Remove(page);
            }
        }

        private void Project_ProjectCreated(object sender, EventArgs e)
        {
            ProjectTitle = String.Format("{0} - {1}", Project.Model.Name, _ProjectTitlePrefix);
        }

        private void Project_ProjectOpened(object sender, EventArgs e)
        {
            ProjectTitle = String.Format("{0} - {1}", Project.Model.Name, _ProjectTitlePrefix);
        }

        private void Project_ProjectClosed(object sender, EventArgs e)
        {
            _Blueprint.Clear();
            _Output.Clear();
            ProjectTitle = _ProjectTitlePrefix;
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
                        RegisterPage(page);

                        WindowMenuItems.Add(new MenuItemViewModel(this, page.ProjectItem.Name, page.SelectPageCommand) { Identifier = page.Identifier });
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
                        UnregisterPage(page);

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

        private void RegisterPage(PageViewModel page)
        {
            page.FileSaved += Page_FileSaved;
            page.FileCloseRequested += Page_CloseFileRequested;
        }

        private void UnregisterPage(PageViewModel page)
        {
            page.FileSaved -= Page_FileSaved;
            page.FileCloseRequested -= Page_CloseFileRequested;
        }

        private void Editor_FileChanged(object sender, FileEventArgs e)
        {
            List<string> scripts = Project.GetAssociatedScripts(e.Path);
            scripts.AddRange(Project.GetImports(e.Path, e.Unsaved).Values);

            EditorViewModel.Completion.ScriptProvider.UpdateVars(scripts);
        }

        private void Backup_RestoreRequested(object sender, BackupRequestedEventArgs e)
        {
            if (Project == null)
            {
                return;
            }

            if (Editor.Items.Count > 0)
            {
                foreach(PageViewModel page in Editor.Items)
                {
                    if (page.Filename == e.Path)
                    {
                        if (page.IsModified)
                        {
                            string message = String.Format("This file ({0}) has unsaved changes. Are you sure you want to restore from backup?", e.Path);
                            if (Services.MessageBox.ShowQuestion(message) != MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                                return;
                            }
                        }

                        Editor.Items.Remove(page);

                        break;
                    }
                }
            }

            File.Write(e.Path, e.Contents);
        }

        public bool FindError(OutputItemViewModel item)
        {
            if (item != null)
            {
                PageViewModel page = null;

                try { page = Editor.Items.Where(i => i.Filename == item.Filename).Single(); } catch { /* if we're here the page is closed */ }
                if (page != null)
                {
                    if (item.Line <= 0 || item.Line > page.Content.Document.LineCount)
                        return false;
                    DocumentLine line = page.Content.Document.GetLineByNumber(item.Line);
                    page.Content.Select(line.Offset, line.Length);
                    page.Content.TextArea.Caret.BringCaretToView();
                    page.IsActive = true;
                    return true;
                }
            }
            return false;
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
                if (!File.Exists(item.Path))
                {
                    return;
                }

                foreach (PageViewModel page in Editor.Items)
                {
                    if (page.Filename == item.Path)
                    {
                        page.IsActive = true;
                        return;
                    }
                }

                PageViewModel newpage = new PageViewModel(this, item);
                Editor.Items.Add(newpage);
            }
        }

        private void Page_FileSaved(object sender, FileEventArgs e)
        {
            // do nothing for now
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
            if (page == null || page.Type == Models.PageType.Output)
            {
                return;
            }

            PerformSaveAll();

            string code = BuildScript(page.Filename);

            if (code != page.Content.Text)
            {
                string tempfile = GetTempFile();
                string tempname = String.Format("{0} - Results View", page.Header);

                File.Write(tempfile, code);

                PageViewModel newpage = new PageViewModel(this, tempname, tempfile)
                {
                    IsReadOnly = true
                };
                newpage.Content.Text = code;

                Editor.Items.Add(newpage);

                page = newpage;
            }

            if (page != null)
            {
                Interop.InGameScript script = new Interop.InGameScript(code);

                string filename = page.Filename;

                Output.Clear();
                
                if (script.CompileErrors.Count > 0)
                {
                    foreach(string error in script.CompileErrors)
                    {
                        string message = error;

                        Match full = _ErrorRegex.Match(error);
                        if (full.Groups.Count > 1)
                        {
                            int diff = Gilgame.SEWorkbench.Interop.InGameScript.HeaderSize;
                            int line = Convert.ToInt32(full.Groups[2].Value) - diff;
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

                RaiseScriptRunning();
            }
        }

        private string GetTempFile()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "temp");

            Directory.CreateDirectory(path);

            return Path.Combine(path, Path.GetRandomFileName());
        }

        private string BuildScript(string path)
        {
            string result = String.Empty;

            ProjectItemViewModel item = Project.GetItemByPath(path);
            if (item != null)
            {
                string code = File.Read(item.Path);

                List<string> scripts = Project.GetAssociatedScripts(path);
                if (scripts.Count < 1)
                {
                    result += code;
                }
                else
                {
                    result += String.Format("{0}{1}{1}", code, Environment.NewLine);
                    foreach (string script in scripts)
                    {
                        result += String.Format("{0}{1}{1}", script, Environment.NewLine);
                    }
                }

                result = Project.ImportReferences(result);
            }

            return result;
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
            if (page != null && page.Type == Models.PageType.Page)
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
                if (page.Type == Models.PageType.Page)
                {
                    page.Save();
                }
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

        public void PerformCloseFile(string path)
        {
            PageViewModel page;
            if (String.IsNullOrEmpty(path))
            {
                page = Editor.SelectedItem;
            }
            else
            {
                page = Editor.Items.Where(i => i.Filename == path).Single();
            }
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

            if (Project.OpenState)
            {
                List<Models.BackupItem> backups = Project.GetBackups();
                foreach (Models.BackupItem backup in backups)
                {
                    Backup.AddItem(backup);
                }

                if (backups.Count > 0)
                {
                    RaiseBackupsFound();
                }
            }

            Project.OpenState = true;

            StartBackupTimer();
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
            StopBackupTimer();

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
