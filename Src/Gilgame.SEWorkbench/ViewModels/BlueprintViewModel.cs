using System;
using System.Windows.Input;
using System.Collections.Generic;

using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class BlueprintViewModel : BaseViewModel
    {
        private IEnumerator<GridItemViewModel> _MatchingItemEnumerator;

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

        public BlueprintViewModel(BaseViewModel parent) : base(parent)
        {
            _First = new Services.ObservableSortedList<GridItemViewModel>(
                new GridItemViewModel[] { },
                new Comparers.GridItemComparer()
            );

            _SearchCommand = new Commands.SearchCommand(this);
        }

        public void SetBlueprint(ObservableSortedList<GridItemViewModel> grid)
        {
            First.Clear();
            if (grid != null)
            {
                if (grid.Count > 0)
                {
                    grid[0].IsExpanded = true;
                    First.Add(grid[0]);
                }
            }
        }

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
    }
}
