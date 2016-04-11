using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels.Comparers
{
    public class BackupItemComparer : IComparer<BackupItemViewModel>
    {
        public int Compare(BackupItemViewModel left, BackupItemViewModel right)
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
                    return left.Path.CompareTo(right.Path);
                }
            }
        }
    }
}
