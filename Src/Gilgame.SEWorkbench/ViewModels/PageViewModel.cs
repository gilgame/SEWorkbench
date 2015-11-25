using System;
using System.Windows;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.CodeCompletion;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        private CSharpCompletion _Completion;
        private CodeTextEditor _Editor = new CodeTextEditor();

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

        public PageViewModel(BaseViewModel parent, string name, string filename) : base(parent)
        {
            _Model = new Models.Page();
            Name = name;
            Header = name;
            Filename = filename;

            BuildEditor();
        }

        private void BuildEditor()
        {
            // TODO make text editor settings gui
            _Editor.FontFamily = new FontFamily("Consolas");
            _Editor.FontSize = 11;
            _Editor.Completion = EditorViewModel.Completion;
            _Editor.OpenFile(Filename);
            _Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            _Editor.Margin = new Thickness(0, 6, 0, 6);
            
            ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };
            _Editor.Options = options;

            _Editor.TextChanged += Editor_TextChanged;

            Content = _Editor;
        }

        private void Editor_TextChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }

        public void Save()
        {
            _Editor.SaveFile();
            IsModified = false;
        }
    }
}
