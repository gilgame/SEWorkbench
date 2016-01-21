using System;
using System.Collections.Generic;
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
        private CodeTextEditor _Editor = new CodeTextEditor();

        public Guid Identifier
        {
            get
            {
                return _Model.Identifier;
            }
            private set
            {
                _Model.Identifier = value;
            }
        }

        private Models.Page _Model;
        public Models.Page Model
        {
            get
            {
                return _Model;
            }
            private set
            {
                _Model = value;
            }
        }

        public string Name
        {
            get
            {
                return _Model.Name;
            }
            private set
            {
                if (_Model.Name != value)
                {
                    _Model.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Filename
        {
            get
            {
                return _Model.Filename;
            }
            set
            {
                if (_Model.Filename != value)
                {
                    _Model.Filename = value;
                    OnPropertyChanged("Filename");
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
                _Model.Type = value;
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
                    OnPropertyChanged("Header");
                }
            }
        }

        private CodeTextEditor _Content;
        public CodeTextEditor Content
        {
            get
            {
                return _Content;
            }
            private set
            {
                if (_Content != value)
                {
                    _Content = value;
                    OnPropertyChanged("Content");
                }
            }
        }

        public string Text
        {
            get
            {
                if (_Content != null)
                {
                    return _Content.Text;
                }
                return String.Empty;
            }
        }

        public string Line
        {
            get
            {
                ICSharpCode.AvalonEdit.Document.DocumentLine line = _Content.Document.GetLineByOffset(_Content.CaretOffset);

                return _Content.Text.Substring(line.Offset, line.Length);
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
                    OnPropertyChanged("IsActive");
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
                    OnPropertyChanged("IsModified");
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

        public event FileEventHandler Selected;
        private void RaiseSelected()
        {
            if (Selected != null)
            {
                Selected(this, new FileEventArgs(Filename));
            }
        }

        public event FileEventHandler FileCloseRequested;
        private void RaiseFileCloseRequested()
        {
            if (FileCloseRequested != null)
            {
                FileCloseRequested(this, new FileEventArgs(_Model.Filename));
            }
        }

        public event FileEventHandler FileSaved;
        private void RaiseFileSaved()
        {
            if (FileSaved != null)
            {
                FileSaved(this, new FileEventArgs(_Model.Filename));
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

        public PageViewModel(BaseViewModel parent, string name, string filename, Models.PageType type) : base(parent)
        {
            _Model = new Models.Page();
            Name = name;
            Header = name;
            Filename = filename;
            Identifier = Guid.NewGuid();
            Type = type;

            BuildEditor();

            _CloseFileCommand = new Commands.CloseFileCommand(this);
            _SelectFileCommand = new Commands.SelectFileCommand(this);
        }

        private void BuildEditor()
        {
            try
            {
                _Editor.OpenFile(Filename);

                // TODO make text editor settings gui
                _Editor.FontFamily = new FontFamily(Configuration.TextEditor.FontFamily);
                _Editor.FontSize = Configuration.TextEditor.FontSize;
                _Editor.Completion = EditorViewModel.Completion;
                _Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
                _Editor.Margin = new Thickness(0, 6, 0, 6);

                ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
                {
                    ConvertTabsToSpaces = Configuration.TextEditor.ConvertTabsToSpaces,
                    IndentationSize = Configuration.TextEditor.TabSize,
                    EnableTextDragDrop = true,
                };
                _Editor.Options = options;

                _Editor.TextChanged += Editor_TextChanged;
                _Editor.TextArea.GotFocus += Editor_GotFocus;

                Content = _Editor;
            }
            catch (Exception ex)
            {
                string message = String.Format("Unable to open file '{0}'", _Editor.FileName);
                Services.MessageBox.ShowError(message, ex);
            }
        }

        public void SilentUnselected()
        {
            _IsSelected = false;
        }

        public void ShowQuickFind()
        {
            foreach (ITextAreaInputHandler handler in _Content.TextArea.DefaultInputHandler.NestedInputHandlers)
            {
                if (handler is SearchInputHandler)
                {
                    return;
                }
            }
            _Content.TextArea.DefaultInputHandler.NestedInputHandlers.Add(new SearchInputHandler(_Content.TextArea));
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

        public void Save()
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

        public void SetReadonly()
        {
            Content.IsReadOnly = true;
        }

        #region Close File Command

        private ICommand _CloseFileCommand;
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

        public void UpdateContent()
        {
            if (!IgnoreUpdates)
            {
                try
                {
                    int start = Content.SelectionStart;

                    Content.OpenFile(Filename);
                    Content.SelectionStart = start;

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

        #endregion

        #region Select File Command

        private ICommand _SelectFileCommand;
        public ICommand SelectFileCommand
        {
            get
            {
                return _SelectFileCommand;
            }
        }

        public void PerformSelectFile()
        {
            IsSelected = true;
        }

        #endregion
    }
}
