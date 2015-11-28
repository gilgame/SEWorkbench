using ICSharpCode.CodeCompletion;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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
                    if (page.IsSelected)
                    {
                        return page;
                    }
                }
                return null;
            }
        }

        public static readonly CSharpCompletion Completion = new ICSharpCode.CodeCompletion.CSharpCompletion(new Completion.ScriptProvider());

        public EditorViewModel(BaseViewModel parent) : base(parent)
        {
            _Items.CollectionChanged += OnCollectionChanged;
        }

        public void InsertText(string text)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Content.Document.Insert(SelectedItem.Content.TextArea.Caret.Offset, text);
                SelectedItem.Content.Focus();
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasChildren");
            
            if (e.NewItems != null)
            {
                if (e.NewItems.Count > 0)
                {
                    if (e.NewItems[0] is PageViewModel)
                    {
                        PageViewModel page = (PageViewModel)e.NewItems[0];
                        page.IsSelected = true;
                        page.Content.Focus();
                    }
                }
            }
        }
    }
}
