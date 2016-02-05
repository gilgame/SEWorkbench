using System;

namespace Gilgame.SEWorkbench.ViewModels.Config
{
    public class ConfigViewModel : BaseViewModel
    {
        private TextEditorViewModel _TextEditor;
        public TextEditorViewModel TextEditor
        {
            get
            {
                return _TextEditor;
            }
            set
            {
                _TextEditor = value;
                RaisePropertyChanged("TextEditor");
            }
        }

        public ConfigViewModel(BaseViewModel parent) : base(parent)
        {
            _TextEditor = new TextEditorViewModel(this);
        }
    }
}
