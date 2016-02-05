using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Search;
using ICSharpCode.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        #region Properties

        private Models.Page _Model;

        public Guid Identifier
        {
            get
            {
                return _Model.Identifier;
            }
            private set
            {
                if (_Model.Identifier != value)
                {
                    _Model.Identifier = value;
                    RaisePropertyChanged("Identifier");
                }
            }
        }

        public string Filename
        {
            get
            {
                if (ProjectItem != null)
                {
                    return ProjectItem.Path;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public Models.PageType Type
        {
            get
            {
                return _Model.Type;
            }
            set
            {
                if (_Model.Type != value)
                {
                    _Model.Type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        private string _Header = "untitled";
        public  string Header
        {
            get
            {
                return _Header;
            }
            private set
            {
                if (_Header != value)
                {
                    _Header = value;
                    RaisePropertyChanged("Header");
                }
            }
        }

        private ProjectItemViewModel _ProjectItem;
        public ProjectItemViewModel ProjectItem
        {
            get
            {
                return _ProjectItem;
            }
        }

        private CodeTextEditor _Editor;
        public CodeTextEditor Content
        {
            get
            {
                return _Editor;
            }
            private set
            {
                if (_Editor != value)
                {
                    _Editor = value;
                    RaisePropertyChanged("Content");
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

                    if (_IsSelected)
                    {
                        RaiseSelected();
                    }

                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        private bool _IsActive = false;
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                if (value != _IsActive)
                {
                    _IsActive = value;

                    if (_IsActive)
                    {
                        RaiseSelected();
                    }

                    RaisePropertyChanged("IsActive");
                }
            }
        }

        private bool _IsModified = false;
        public bool IsModified
        {
            get
            {
                return _IsModified;
            }
            set
            {
                if (value != _IsModified)
                {
                    _IsModified = value;
                    RaisePropertyChanged("IsModified");
                }
            }
        }

        private bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                if (_Editor != null)
                {
                    _Editor.IsReadOnly = value;
                }
                if (value != _IsReadOnly)
                {
                    _IsReadOnly = value;
                    RaisePropertyChanged("IsReadOnly");
                }
            }
        }

        private bool _IgnoreUpdates = false;
        public bool IgnoreUpdates
        {
            get
            {
                return _IgnoreUpdates;
            }
            set
            {
                _IgnoreUpdates = value;
            }
        }

        private DateTime _LastSaved = DateTime.Now;
        public DateTime LastSaved
        {
            get
            {
                return _LastSaved;
            }
        }

        #endregion

        #region Events

        public event FileEventHandler Selected;
        private void RaiseSelected()
        {
            if (Selected != null)
            {
                Selected(this, new FileEventArgs(ProjectItem.Path));
            }
        }

        public event FileEventHandler FileSaved;
        private void RaiseFileSaved()
        {
            if (FileSaved != null)
            {
                FileSaved(this, new FileEventArgs(ProjectItem.Path));
            }
        }

        public event EventHandler TextChanged;
        private void RaiseTextChanged()
        {
            if (TextChanged != null)
            {
                TextChanged(this, EventArgs.Empty);
            }
        }

        public event FileEventHandler FileCloseRequested;
        private void RaiseFileCloseRequested()
        {
            if (FileCloseRequested != null)
            {
                FileCloseRequested(this, new FileEventArgs(ProjectItem.Path));
            }
        }

        #endregion

        #region Constructor

        public PageViewModel(BaseViewModel parent, ProjectItemViewModel item) : base(parent)
        {
            Initialize(parent, item, Models.PageType.Page);
        }

        public PageViewModel(BaseViewModel parent, string name, string path) : base(parent)
        {
            ProjectItemViewModel item = new ProjectItemViewModel(null)
            {
                Name = name,
                Path = path
            };
            Initialize(parent, item, Models.PageType.Output);
        }

        private void Initialize(BaseViewModel parent, ProjectItemViewModel item, Models.PageType type)
        {
            _ProjectItem = item;

            _Model = new Models.Page();
            Header = item.Name;
            Identifier = Guid.NewGuid();
            Type = type;

            item.PropertyChanged += ProjectItem_PropertyChanged;

            BuildEditor();

            _CloseFileCommand = new Commands.CloseFileCommand(this);
            _SelectPageCommand = new Commands.SelectPageCommand(this);
        }

        private void ProjectItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Path")
            {
                _Editor.FileName = ProjectItem.Path;
            }
            if (e.PropertyName == "Name")
            {
                Header = ProjectItem.Name;
            }
        }

        private void BuildEditor()
        {
            CodeTextEditor editor = new CodeTextEditor();

            try
            {
                editor.OpenFile(ProjectItem.Path);

                editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
                editor.Margin = new Thickness(0, 6, 0, 6);

                editor.IsReadOnly = _IsReadOnly;

                editor.TextChanged += Editor_TextChanged;
                editor.TextArea.GotFocus += Editor_GotFocus;

                editor.TextArea.DefaultInputHandler.NestedInputHandlers.Add(new SearchInputHandler(editor.TextArea));

                Content = editor;

                UpdateEditorConfig();
            }
            catch (Exception ex)
            {
                string message = String.Format("Unable to open file '{0}'", editor.FileName);
                Services.MessageBox.ShowError(message, ex);
            }
        }

        public void UpdateEditorConfig()
        {
            _Editor.FontFamily = new FontFamily(Configuration.TextEditor.FontFamily);
            _Editor.FontSize = Configuration.TextEditor.FontSize;
            _Editor.Completion = EditorViewModel.Completion;

            ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
            {
                ConvertTabsToSpaces = Configuration.TextEditor.ConvertTabsToSpaces,
                IndentationSize = Configuration.TextEditor.TabSize,
                EnableTextDragDrop = true,
            };
            _Editor.Options = options;
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            if (Type == Models.PageType.Page)
            {
                IsModified = true;
            }
            RaiseTextChanged();
        }

        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            IsActive = true;
        }

        #endregion

        public void Save()
        {
            if (_IsModified)
            {
                try
                {
                    if (_Editor.SaveFile())
                    {
                        IsModified = false;
                        IgnoreUpdates = false;

                        _LastSaved = DateTime.Now;

                        RaiseFileSaved();
                    }
                }
                catch (Exception ex)
                {
                    string message = String.Format("Failed to save file '{0}'", _Editor.FileName);
                    Services.MessageBox.ShowError(message, ex);
                }
            }
        }

        public void UpdateContent()
        {
            if (!IgnoreUpdates)
            {
                try
                {
                    int start = _Editor.SelectionStart;

                    _Editor.OpenFile(ProjectItem.Path);
                    _Editor.SelectionStart = start;

                    _Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");

                    IsModified = false;
                }
                catch (Exception ex)
                {
                    string message = String.Format("Unable to open file '{0}'", _Editor.FileName);
                    Services.MessageBox.ShowError(message, ex);
                }
            }
        }

        public void ShowQuickFind()
        {
            foreach (ITextAreaInputHandler handler in _Editor.TextArea.DefaultInputHandler.NestedInputHandlers)
            {
                if (handler is SearchInputHandler)
                {
                    _Editor.TextArea.Focus();

                    SearchInputHandler search = (SearchInputHandler)handler;
                    search.Open();
                }
            }
        }

        #region Commands

        #region Select Page Command

        private ICommand _SelectPageCommand;
        public ICommand SelectPageCommand
        {
            get
            {
                return _SelectPageCommand;
            }
        }

        public void PerformSelectPage()
        {
            IsSelected = true;
            IsActive = true;
            _Editor.TextArea.Focus();
        }

        #endregion

        #region Close File Command

        private ICommand _CloseFileCommand;

        /// <summary>
        /// Required for AvalonDock compatability
        /// </summary>
        public ICommand CloseFileCommand
        {
            get
            {
                return _CloseFileCommand;
            }
        }

        public void PerformCloseFile()
        {
            RaiseFileCloseRequested();
        }

        #endregion

        #endregion
    }
}
