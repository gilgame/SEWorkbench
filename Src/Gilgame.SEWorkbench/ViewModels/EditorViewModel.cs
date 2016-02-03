using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using ICSharpCode.CodeCompletion;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class EditorViewModel : BaseViewModel
    {
        private ObservableCollection<PageViewModel> _Items = new ObservableCollection<PageViewModel>();
        public ObservableCollection<PageViewModel> Items
        {
            get
            {
                return _Items;
            }
        }
        
        public bool HasChildren
        {
            get
            {
                if (_Items == null)
                {
                    return false;
                }
                return _Items.Count > 0;
            }
        }

        public PageViewModel SelectedItem
        {
            get
            {
                foreach (PageViewModel page in Items)
                {
                    if (page.IsSelected)
                    {
                        return page;
                    }
                }
                return null;
            }
        }

        public static Interop.ScriptProvider ScriptProvider = new Interop.ScriptProvider();
        public static CSharpCompletion Completion = new ICSharpCode.CodeCompletion.CSharpCompletion(ScriptProvider);

        public event FileEventHandler FileChanged;
        private void RaiseFileChanged(string path)
        {
            if (FileChanged != null)
            {
                FileChanged(this, new FileEventArgs(path));
            }
        }

        public EditorViewModel(BaseViewModel parent) : base(parent)
        {
            _UpdateAutoCompleteCommand = new Commands.UpdateAutoCompleteCommand(this);
            _ShowQuickFindCommand = new Commands.ShowQuickFindCommand(this);

            _Items.CollectionChanged += OnCollectionChanged;
        }

        private void Page_Selected(object sender, FileEventArgs e)
        {
            UpdateScriptProvider();
        }

        private void Page_FileSaved(object sender, EventArgs e)
        {
            RaiseFileChanged(SelectedItem.Filename);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                if (e.NewItems.Count == 1)
                {
                    if (e.NewItems[0] is PageViewModel)
                    {
                        PageViewModel page = (PageViewModel)e.NewItems[0];
                        RegisterPage(page);

                        page.IsActive = true;
                    }
                }
                // shouldn't reach here,but just in case
                if (e.NewItems.Count > 1)
                {
                    foreach(object o in e.NewItems)
                    {
                        if (o is PageViewModel)
                        {
                            PageViewModel page = (PageViewModel)o;
                            RegisterPage(page);
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (object o in e.OldItems)
                {
                    if (o is PageViewModel)
                    {
                        PageViewModel page = (PageViewModel)o;
                        UnregisterPage(page);
                    }
                }
            }

            OnPropertyChanged("HasChildren");
        }

        private void RegisterPage(PageViewModel page)
        {
            page.Selected += Page_Selected;
            page.FileSaved += Page_FileSaved;
        }

        private void UnregisterPage(PageViewModel page)
        {
            page.Selected -= Page_Selected;
            page.FileSaved -= Page_FileSaved;
        }

        public void InsertText(string text)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Content.Document.Insert(SelectedItem.Content.TextArea.Caret.Offset, text);
                SelectedItem.Content.Focus();
            }
        }

        private void UpdateScriptProvider()
        {
            if (SelectedItem != null)
            {
                RaiseFileChanged(SelectedItem.Filename);
            }
        }

        #region Commands

        #region Update Auto-Complete Command

        private readonly ICommand _UpdateAutoCompleteCommand;
        public ICommand UpdateAutoCompleteCommand
        {
            get
            {
                return _UpdateAutoCompleteCommand;
            }
        }

        public void PerformUpdateAutoComplete()
        {
            RaiseFileChanged(SelectedItem.Filename);
        }

        #endregion

        #region Show Quick-Find Command

        private readonly ICommand _ShowQuickFindCommand;
        public ICommand ShowQuickFindCommand
        {
            get
            {
                return _ShowQuickFindCommand;
            }
        }

        public void PerformShowQuickFind()
        {
            PageViewModel selected = SelectedItem;
            if (selected != null)
            {
                selected.ShowQuickFind();
            }
        }

        #endregion

        #endregion
    }
}
