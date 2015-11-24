using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels.Comparers
{
    public class ProjectItemComparer<T> : IComparer<T>
    {
        int IComparer<T>.Compare(T a, T b)
        {
            if (a is ProjectItemViewModel)
            {
                ProjectItemViewModel left = (ProjectItemViewModel)(object)a;
                ProjectItemViewModel right = (ProjectItemViewModel)(object)b;

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
                        if (IsDirectory(left))
                        {
                            if (IsDirectory(right))
                            {
                                if (left.Type == Models.ProjectItemType.Blueprints)
                                {
                                    if (right.Type == Models.ProjectItemType.Blueprints)
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
                                    if (right.Type == Models.ProjectItemType.Blueprints)
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
                                return -1;
                            }
                        }
                        else
                        {
                            if (IsDirectory(right))
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
            else
            {
                return 0;
            }
        }

        private bool IsDirectory(ProjectItemViewModel item)
        {
            switch (item.Type)
            {
                case Models.ProjectItemType.Root:
                case Models.ProjectItemType.Blueprints:
                case Models.ProjectItemType.Folder:
                    return true;

                default:
                    return false;
            }
        }
    }
}
