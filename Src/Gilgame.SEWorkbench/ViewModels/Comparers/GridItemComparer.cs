using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels.Comparers
{
    public class GridItemComparer<T> : IComparer<T>
    {
        int IComparer<T>.Compare(T a, T b)
        {
            if (a is GridItemViewModel)
            {
                GridItemViewModel left = (GridItemViewModel)(object)a;
                GridItemViewModel right = (GridItemViewModel)(object)b;

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
            else
            {
                return 0;
            }
        }
    }
}
