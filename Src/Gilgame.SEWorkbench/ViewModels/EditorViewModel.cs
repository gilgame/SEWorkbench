using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using ICSharpCode.CodeCompletion;

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
                foreach(PageViewModel page in Items)
                {
                    if (page.IsActive)
                    {
                        return page;
                    }
                }
                return null;
            }
        }

        public static Completion.ScriptProvider ScriptProvider = new Completion.ScriptProvider();
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
            _Items.CollectionChanged += OnCollectionChanged;
        }

        private void Page_Selected(object sender, FileEventArgs e)
        {
            // wpf sucks at setting other items in the collection back to IsSelected = false,
            // so we'll do it quietly as not to trigger all the bindings again
            //foreach (PageViewModel page in Items)
            //{
            //    if (page.Filename != e.Path)
            //    {
            //        page.SilentUnselected();
            //    }
            //}

            UpdateScriptProvider();
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
        }

        private void UnregisterPage(PageViewModel page)
        {
            page.Selected -= Page_Selected;
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
    }
}
