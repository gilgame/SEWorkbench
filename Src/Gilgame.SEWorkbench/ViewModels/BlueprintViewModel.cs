using System;
using System.Windows.Input;
using System.Collections.Generic;

using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class BlueprintViewModel : BaseViewModel
    {
        private IEnumerator<GridItemViewModel> _MatchingItemEnumerator;
        private IEnumerator<GridItemViewModel> _SelectedItemEnumerator;

        private ObservableSortedList<GridItemViewModel> _First;
        public ObservableSortedList<GridItemViewModel> First
        {
            get
            {
                return _First;
            }
        }

        private string _SearchText = String.Empty;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                if (value == _SearchText)
                {
                    return;
                }

                _SearchText = value;
                _MatchingItemEnumerator = null;
            }
        }

        public GridItemViewModel SelectedItem
        {
            get
            {
                return FindSelectedItem();
            }
        }

        public Models.GridItemType SelectedItemType
        {
            get
            {
                return (SelectedItem == null) ? Models.GridItemType.None : SelectedItem.Type;
            }
        }

        public GridItemViewModel RootItem
        {
            get
            {
                if (First != null && First.Count > 0)
                {
                    return First[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public BlueprintViewModel(BaseViewModel parent) : base(parent)
        {
            _First = new Services.ObservableSortedList<GridItemViewModel>(
                new GridItemViewModel[] { },
                new Comparers.GridItemComparer()
            );

            _SearchCommand = new Commands.SearchCommand(this);
            _InsertNameCommand = new Commands.InsertNameCommand(this);
            _EditProgramCommand = new Commands.EditProgramCommand(this);
            _RenameBlockCommand = new Commands.RenameBlockCommand(this);
        }
        
        public event EventHandler SelectionChanged;
        public void RaiseSelectionChanged()
        {
            OnPropertyChanged("SelectedItemType");
        }

        public event InsertEventHandler InsertRequested;
        public void RaiseInsertRequested()
        {
            if (SelectedItemType == Models.GridItemType.Block)
            {
                if (InsertRequested != null)
                {
                    InsertRequested(this, new InsertEventArgs(SelectedItem.Name));
                }
            }
        }

        public void SetBlueprint(ObservableSortedList<GridItemViewModel> grid)
        {
            First.Clear();
            if (grid != null)
            {
                if (grid.Count > 0)
                {
                    SetBlueprint(grid[0]);
                    grid[0].IsExpanded = true;
                    First.Add(grid[0]);
                }
            }
        }

        public void Clear()
        {
            First.Clear();
        }

        private void SetBlueprint(GridItemViewModel root)
        {
            if (root != null)
            {
                root.Model.Blueprint = this;
                foreach(GridItemViewModel item in root.Children)
                {
                    SetBlueprint(item);
                }
            }
        }

        #region FindSelectedItem

        private GridItemViewModel FindSelectedItem()
        {
            VerifySelectedItemEnumerator();

            return _SelectedItemEnumerator.Current;
        }

        private void VerifySelectedItemEnumerator()
        {
            GridItemViewModel root = RootItem;

            var matches = FindSelected(root);

            _SelectedItemEnumerator = matches.GetEnumerator();
            if (!_SelectedItemEnumerator.MoveNext())
            {
                // none selected
            }
        }

        private IEnumerable<GridItemViewModel> FindSelected(GridItemViewModel item)
        {
            if (item == null)
            {
                yield return null;
            }

            if (item.IsSelected)
            {
                yield return item;
            }

            foreach (GridItemViewModel child in item.Children)
            {
                foreach (GridItemViewModel match in FindSelected(child))
                {
                    // TODO fix collection modified exception
                    yield return match;
                }
            }
        }

        #endregion

        #region Search Command

        private readonly ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand;
            }
        }

        public void PerformSearch()
        {
            if (_MatchingItemEnumerator == null || !_MatchingItemEnumerator.MoveNext())
            {
                VerifyMatchingItemEnumerator();
            }
            if (_MatchingItemEnumerator == null)
            {
                return;
            }

            var item = _MatchingItemEnumerator.Current;
            if (item == null)
            {
                return;
            }

            if (item.Parent != null)
            {
                GridItemViewModel parent = (GridItemViewModel)item.Parent;
                parent.IsExpanded = true;
            }

            // TODO switch search to filter (hide unmatched items), probably handled by the UI instead

            item.IsSelected = true;
        }

        private void VerifyMatchingItemEnumerator()
        {
            if (First == null || First.Count < 1)
            {
                return;
            }

            var matches = FindMatches(_SearchText, First[0]);

            _MatchingItemEnumerator = matches.GetEnumerator();
            if (!_MatchingItemEnumerator.MoveNext())
            {
                // none found, do nothing for now
            }
        }

        private IEnumerable<GridItemViewModel> FindMatches(string text, GridItemViewModel item)
        {
            if (item == null)
            {
                yield return null;
            }
            if (item.NameContainsText(text))
            {
                yield return item;
            }

            foreach (GridItemViewModel child in item.Children)
            {
                foreach (GridItemViewModel match in FindMatches(text, child))
                {
                    yield return match;
                }
            }
        }

        #endregion

        #region Edit Program Command

        private readonly ICommand _EditProgramCommand;
        public ICommand EditProgramCommand
        {
            get
            {
                return _EditProgramCommand;
            }
        }

        public void PerformEditProgram()
        {
            if (SelectedItemType != Models.GridItemType.Program)
            {
                return;
            }

            GridItemViewModel selected = SelectedItem;

            Views.ProgramView dialog = new Views.ProgramView()
            {
                Program = selected.Program,
                Blueprint = RootItem.Name,
                Block = selected.Name,
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string program = dialog.Program;

                RootItem.Definitions = Interop.Blueprint.SaveProgram(RootItem.Path, RootItem.Definitions, selected.EntityID, program);

                selected.Program = program;
            }
        }

        #endregion

        #region Rename Block Command

        private readonly ICommand _RenameBlockCommand;
        public ICommand RenameBlockCommand
        {
            get
            {
                return _RenameBlockCommand;
            }
        }

        public void PerformRenameBlock()
        {
            if (SelectedItemType != Models.GridItemType.Block)
            {
                return;
            }

            GridItemViewModel selected = SelectedItem;

            Views.RenameDialog dialog = new Views.RenameDialog()
            {
                ItemName = selected.Name
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                string name = dialog.ItemName;
                try
                {
                    RootItem.Definitions = Interop.Blueprint.SaveBlockName(RootItem.Path, RootItem.Definitions, selected.EntityID, name);

                    selected.Name = name;
                }
                catch (Exception ex)
                {
                    MessageBox.ShowError("Unable to change block name", ex);
                }
            }
        }

        #endregion

        #region Insert Name Command

        private readonly ICommand _InsertNameCommand;
        public ICommand InsertNameCommand
        {
            get
            {
                return _InsertNameCommand;
            }
        }

        public void PerformInsertName()
        {
            RaiseInsertRequested();
        }

        #endregion
    }
}
