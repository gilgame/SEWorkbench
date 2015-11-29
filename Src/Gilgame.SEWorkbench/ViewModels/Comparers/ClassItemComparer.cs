using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels.Comparers
{
    public class ClassItemComparer : IComparer<ClassItemViewModel>
    {
        public int Compare(ClassItemViewModel left, ClassItemViewModel right)
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
                    if (left.Type == Models.ClassItemType.Field)
                    {
                        if (right.Type == Models.ClassItemType.Field)
                        {
                            return left.Name.CompareTo(right.Name);
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (right.Type == Models.ClassItemType.Field)
                        {
                            return 1;
                        }
                        else
                        {
                            if (left.Type == Models.ClassItemType.Property)
                            {
                                if (right.Type == Models.ClassItemType.Property)
                                {
                                    return left.Name.CompareTo(right.Name);
                                }
                                else
                                {
                                    return -1;
                                }
                            }
                            else
                            {
                                if (right.Type == Models.ClassItemType.Property)
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
            }
        }
    }
}
