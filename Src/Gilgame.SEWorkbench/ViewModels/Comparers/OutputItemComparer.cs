using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels.Comparers
{
    public class OutputItemComparer : IComparer<OutputItemViewModel>
    {
        public int Compare(OutputItemViewModel left, OutputItemViewModel right)
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
                    return left.Line.CompareTo(right.Line);
                }
            }
        }
    }
}
