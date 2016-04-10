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

        private UpdaterViewModel _Updater;
        public UpdaterViewModel Updater
        {
            get
            {
                return _Updater;
            }
            set
            {
                _Updater = value;
                RaisePropertyChanged("Updater");
            }
        }

        public ConfigViewModel(BaseViewModel parent) : base(parent)
        {
            _TextEditor = new TextEditorViewModel(this);
            _Updater = new UpdaterViewModel(this);
        }
    }
}
