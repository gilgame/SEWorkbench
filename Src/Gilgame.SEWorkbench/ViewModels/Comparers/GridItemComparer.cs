using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels.Comparers
{
    public class GridItemComparer : IComparer<GridItemViewModel>
    {
        public int Compare(GridItemViewModel left, GridItemViewModel right)
        {
            if (left == null)
            {
                if (right == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (right == null)
                {
                    return 1;
                }
                else
                {
                    return left.Name.CompareTo(right.Name);
                }
            }
        }
    }
}
