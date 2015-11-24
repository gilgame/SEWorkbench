using System;

using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench.ViewModels
{
    public class BlueprintViewModel : BaseViewModel
    {
        private ObservableSortedList<GridItemViewModel> _First;
        public ObservableSortedList<GridItemViewModel> First
        {
            get
            {
                return _First;
            }
        }

        public BlueprintViewModel(BaseViewModel parent) : base(parent)
        {
            _First = new Services.ObservableSortedList<GridItemViewModel>(
                new GridItemViewModel[] { },
                new Comparers.ProjectItemComparer<GridItemViewModel>()
            );
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
    }
}
