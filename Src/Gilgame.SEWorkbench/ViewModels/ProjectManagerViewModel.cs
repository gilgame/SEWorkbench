using Gilgame.SEWorkbench.Services;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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

        private ObservableCollection<MenuItemViewModel> _RootMenuItem;
        public ObservableCollection<MenuItemViewModel> RootMenuItem
        {
            get
            {
                return _RootMenuItem;
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

        public ProjectManagerViewModel(BaseViewModel parent) : base(parent)
        {
            // create project object first
            Project = new ProjectViewModel(this);
            Project.FileRequested += Project_FileRequested;
            Project.SelectionChanged += Project_SelectionChanged;

            Blueprint = new BlueprintViewModel(this);
            Editor = new EditorViewModel(this);
            Output = new OutputViewModel(this);

            _SaveFileCommand = new Commands.SaveFileCommand(this);
            _SaveAllCommand = new Commands.SaveAllCommand(this);

            _RunScriptCommand = new Commands.RunScriptCommand(this);
            _OpenSelectedCommand = new Commands.OpenSelectedCommand(this);

            BuildMenu();
        }

        private void Project_SelectionChanged(object sender, EventArgs e)
        {
            ProjectItemViewModel item = Project.GetSelectedBlueprint();
            if (item != null)
            {
                Blueprint.SetBlueprint(item.Grid);
            }
        }

        private void Project_FileRequested(object sender, EventArgs e)
        {
            PerformOpenSelected();
        }

        #region Build Menu

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
                    mopen.AddChild(new MenuItemViewModel(mopen, "Project...", _Project.OpenProjectCommand) { InputGestureText = "Ctrl+Shift+O" });
                }
                file.AddChild(mopen);

                file.AddSeparator();

                file.AddChild(new MenuItemViewModel(file, "Close", _Project.CloseProjectCommand));

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
                window.AddChild(new MenuItemViewModel(window, "Close"));
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

        #region Open Selected

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
            if (item != null)
            {
                _Editor.OpenItem(item);
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

                Output.Clear();
                
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
                                Message = errmsg
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

                                Output.AddItem(new Models.OutputItem() { Line = line, Column = col, Message = message });
                            }
                            else
                            {
                                Output.AddItem(new Models.OutputItem() { Message = message });
                            }
                        }
                    }

                    Views.OutputView view = new Views.OutputView();
                    view.DataContext = Output;
                    view.ShowDialog();
                }
                else
                {
                    MessageBox.ShowMessage("The program compiled without any errors.");
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
    }
}
