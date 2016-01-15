using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ViewModels.Comparers
{
    public class ProjectItemComparer : IComparer<ProjectItemViewModel>
    {
        public int Compare(ProjectItemViewModel left, ProjectItemViewModel right)
        {
            if (left.Type == Models.ProjectItemType.References)
            {
                return -1;
            }
            else
            {
                if (right.Type == Models.ProjectItemType.References)
                {
                    return 1;
                }

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
