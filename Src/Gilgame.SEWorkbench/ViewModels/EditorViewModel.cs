using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

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
                foreach (PageViewModel page in Items)
                {
                    if (page.IsActive)
                    {
                        return page;
                    }
                }
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
        private void RaiseFileChanged(string path, bool unsaved)
        {
            if (FileChanged != null)
            {
                FileChanged(this, new FileEventArgs(path, unsaved));
            }
        }

        public EditorViewModel(BaseViewModel parent) : base(parent)
        {
            _UpdateAutoCompleteCommand = new Commands.DelegateCommand(PerformUpdateAutoComplete);
            _ShowQuickFindCommand = new Commands.DelegateCommand(PerformShowQuickFind);

            _Items.CollectionChanged += OnCollectionChanged;

            if (parent != null && parent is ProjectManagerViewModel)
            {
                ProjectManagerViewModel manager = (ProjectManagerViewModel)parent;
                manager.Config.TextEditor.PropertyChanged += Config_PropertyChanged;
            }
        }

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach(PageViewModel page in Items)
            {
                page.UpdateEditorConfig();
            }
        }

        private void Page_Selected(object sender, FileEventArgs e)
        {
            UpdateScriptProvider();
        }

        private void Page_FileSaved(object sender, EventArgs e)
        {
            RaiseFileChanged(SelectedItem.Filename, false);
        }

        private void Page_TextChanged(object sender, EventArgs e)
        {
            //var currentLine = _editor.Document.GetLineByOffset(_editor.CaretOffset);

            //PageViewModel page = SelectedItem;
            //if (page != null)
            //{
            //    var line = page.Content.Document.GetLineByOffset(page.Content.CaretOffset);
            //    if (line != null)
            //    {
            //        string text = page.Content.Text.Substring(line.Offset, line.Length).TrimStart();
            //        if (text.StartsWith("#import "))
            //        {
                        PerformUpdateAutoComplete(true);
            //        }
            //    }
            //}
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
                    PageViewModel last = (PageViewModel)e.NewItems[e.NewItems.Count-1];
                    last.IsActive = true;
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

            RaisePropertyChanged("HasChildren");
        }

        private void RegisterPage(PageViewModel page)
        {
            page.Selected += Page_Selected;
            page.FileSaved += Page_FileSaved;
            page.TextChanged += Page_TextChanged;
        }

        private void UnregisterPage(PageViewModel page)
        {
            page.Selected -= Page_Selected;
            page.FileSaved -= Page_FileSaved;
            page.TextChanged -= Page_TextChanged;
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
                RaiseFileChanged(SelectedItem.Filename, false);
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

        public void PerformUpdateAutoComplete(bool unsaved = false)
        {
            RaiseFileChanged(SelectedItem.Filename, unsaved);
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
