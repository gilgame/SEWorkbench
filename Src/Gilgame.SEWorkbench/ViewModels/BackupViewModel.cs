using System;
using System.Collections.Generic;
using System.Windows.Input;

using Gilgame.SEWorkbench.Services;
using Gilgame.SEWorkbench.Services.IO;

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

        public event BackupRequestedEventHandler RestoreRequested;
        private void RaiseRestoreRequested(string original, string contents)
        {
            var handler = RestoreRequested;
            if (handler != null)
            {
                BackupRequestedEventArgs e = new BackupRequestedEventArgs(original, contents);

                handler(this, e);

                if (!e.Cancel)
                {
                    RemoveItem(original);
                }
            }
        }

        public BackupViewModel(BaseViewModel parent) : base(parent)
        {
            _Items = new Services.ObservableSortedList<BackupItemViewModel>(
                new Comparers.BackupItemComparer()
            );

            _RestoreAllCommand = new Commands.DelegateCommand(PerformRestoreAll);
            _ClearAllCommand = new Commands.DelegateCommand(PerformClearAll);
            _RestoreCommand = new Commands.DelegateCommand(PerformRestore);
            _DeleteCommand = new Commands.DelegateCommand(PerformDelete);
        }

        public void AddItem(Models.BackupItem model)
        {
            BackupItemViewModel item = new BackupItemViewModel(model, this);
            Items.Add(item);
        }

        public void RemoveItem(string original)
        {
            foreach (BackupItemViewModel item in _Items)
            {
                if (item.Original == original)
                {
                    if (File.Exists(item.Path))
                    {
                        File.Delete(item.Path);
                    }

                    _Items.Remove(item);
                    break;
                }
            }
        }

        public void Clear()
        {
            foreach (BackupItemViewModel item in _Items)
            {
                if (File.Exists(item.Path))
                {
                    File.Delete(item.Path);
                }
            }

            Items.Clear();
        }

        #region Restore All Command

        private readonly ICommand _RestoreAllCommand;
        public ICommand RestoreAllCommand
        {
            get
            {
                return _RestoreAllCommand;
            }
        }

        public void PerformRestoreAll()
        {
            string message = "Restore All: Are you sure want to restore all backups?";
            if (MessageBox.ShowQuestion(message) != System.Windows.MessageBoxResult.Yes)
            {
                return;
            }

            List<BackupItemViewModel> items = new List<BackupItemViewModel>();
            items.AddRange(_Items);

            foreach (BackupItemViewModel item in items)
            {
                RaiseRestoreRequested(item.Original, item.Contents);
            }
        }

        #endregion

        #region Clear All Command

        private readonly ICommand _ClearAllCommand;
        public ICommand ClearAllCommand
        {
            get
            {
                return _ClearAllCommand;
            }
        }

        public void PerformClearAll()
        {
            string message = "Delete All: Do you really want to delete all backups for this project? This cannot be undone.";
            if (MessageBox.ShowQuestion(message) != System.Windows.MessageBoxResult.Yes)
            {
                return;
            }

            Clear();
        }

        #endregion

        #region Restore Command

        private readonly ICommand _RestoreCommand;
        public ICommand RestoreCommand
        {
            get
            {
                return _RestoreCommand;
            }
        }

        private void PerformRestore(string original)
        {
            string message = String.Format("Restore: Are you sure you want to restore this file ({0})?", original);
            if (MessageBox.ShowQuestion(message) != System.Windows.MessageBoxResult.Yes)
            {
                return;
            }

            BackupItemViewModel backup = null;
            foreach(BackupItemViewModel item in _Items)
            {
                if (item.Original == original)
                {
                    backup = item;
                    break;
                }
            }

            if (backup != null)
            {
                RaiseRestoreRequested(original, backup.Contents);
            }
        }

        #endregion

        #region Delete Command

        private readonly ICommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _DeleteCommand;
            }
        }

        public void PerformDelete(string original)
        {
            string message = String.Format("Delete: Do you really want to delete the backup for this file ({0})?", original);
            if (MessageBox.ShowQuestion(message) != System.Windows.MessageBoxResult.Yes)
            {
                return;
            }

            BackupItemViewModel backup = null;
            foreach (BackupItemViewModel item in _Items)
            {
                if (item.Original == original)
                {
                    backup = item;
                    break;
                }
            }

            if (backup != null)
            {
                if (File.Exists(backup.Path))
                {
                    File.Delete(backup.Path);
                }
            }

            RemoveItem(original);
        }

        #endregion
    }
}
