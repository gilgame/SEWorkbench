using System;

using ICSharpCode.CodeCompletion;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Windows;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        private ICSharpCode.CodeCompletion.CSharpCompletion _Completion;

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

        public PageViewModel(BaseViewModel parent, string name, string filename) : base(parent)
        {
            _Model = new Models.Page();
            Name = name;
            Filename = filename;

            BuildEditor();
        }

        private void BuildEditor()
        {
            _Completion = new ICSharpCode.CodeCompletion.CSharpCompletion(new Completion.ScriptProvider());

            CodeTextEditor editor = new CodeTextEditor();
            editor.FontFamily = new FontFamily("Consolas");
            editor.FontSize = 11;
            editor.Completion = _Completion;
            //editor.OpenFile(Filename);
            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            editor.Margin = new Thickness(0, 6, 0, 6);

            ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };
            editor.Options = options;

            Content = editor;
        }
    }
}
