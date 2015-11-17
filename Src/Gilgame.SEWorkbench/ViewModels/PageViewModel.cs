using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.CodeCompletion;
using System;
using System.ComponentModel;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class PageViewModel : INotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    if (_Name != value)
                    {
                        _Name = value;
                        OnPropertyChanged("Name");
                    }
                }
            }
        }

        private string _Filename = String.Empty;
        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                if (_Filename != value)
                {
                    _Filename = value;
                    OnPropertyChanged("Filename");
                }
            }
        }

        private System.Windows.Media.FontFamily _FontFamily = new System.Windows.Media.FontFamily("Consolas");
        public System.Windows.Media.FontFamily FontFamily
        {
            get
            {
                return _FontFamily;
            }
            set
            {
                if (_FontFamily != value)
                {
                    _FontFamily = value;
                    OnPropertyChanged("FontFamily");
                }
            }
        }

        private double _FontSize = 11;
        public double Fontsize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                if (_FontSize != value)
                {
                    _FontSize = value;
                    OnPropertyChanged("Fontsize");
                }
            }
        }

        private IHighlightingDefinition _SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
        public IHighlightingDefinition SyntaxHighlighting
        {
            get
            {
                return _SyntaxHighlighting;
            }
            set
            {
                if (_SyntaxHighlighting != value)
                {
                    _SyntaxHighlighting = value;
                    OnPropertyChanged("SyntaxHighlighting");
                }
            }
        }

        public ViewModels.EditorViewModel Editor { get; set; }

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

                    if (Editor != null)
                    {
                        Editor.SelectionChanged();
                    }
                }
            }
        }

        private CSharpCompletion _Completion = new CSharpCompletion(new Completion.ScriptProvider());
        public CSharpCompletion Completion
        {
            get
            {
                return _Completion;
            }
        }

        public PageViewModel(string name)
        {
            _Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
