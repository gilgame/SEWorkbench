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

        public EditorViewModel(BaseViewModel parent) : base(parent)
        {
            _Items.CollectionChanged += OnCollectionChanged;
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
                    }
                }
            }
        }

        public void OpenItem(ProjectItemViewModel item)
        {
            if (item != null)
            {
                PageViewModel page = new PageViewModel(this, item.Name, item.Path);
                Items.Add(page);
            }
        }
    }
}
