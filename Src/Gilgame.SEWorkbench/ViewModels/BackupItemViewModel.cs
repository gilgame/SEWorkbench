using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class BackupItemViewModel : BaseViewModel
    {
        private Models.BackupItem _Model;
        public Models.BackupItem Model
        {
            get
            {
                return _Model;
            }
        }

        public string Name
        {
            get
            {
                return _Model.Name;
            }
        }

        public string Path
        {
            get
            {
                return _Model.Path;
            }
        }

        public string Original
        {
            get
            {
                return _Model.Original;
            }
        }

        public DateTime? Modified
        {
            get
            {
                return _Model.Modified;
            }
        }

        public string Contents
        {
            get
            {
                return _Model.Contents;
            }
        }

        public BackupItemViewModel(Models.BackupItem item, BaseViewModel parent) : base(parent)
        {
            _Model = item;
        }
    }
}
