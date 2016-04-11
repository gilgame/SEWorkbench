using System;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private Services.ObservableSortedList<BackupItemViewModel> _Items;
        public Services.ObservableSortedList<BackupItemViewModel> Items
        {
            get
            {
                return _Items;
            }
        }

        public BackupViewModel(BaseViewModel parent) : base(parent)
        {
            _Items = new Services.ObservableSortedList<BackupItemViewModel>(
                new Comparers.BackupItemComparer()
            );
        }

        public void AddItem(Models.BackupItem model)
        {
            BackupItemViewModel item = new BackupItemViewModel(model, this);
            Items.Add(item);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
