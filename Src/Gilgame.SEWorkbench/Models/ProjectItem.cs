using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gilgame.SEWorkbench.Models
{
    public class ProjectItem
    {
        public string Name { get; set; }

        public ProjectItemType Type { get; set; }

        private ObservableCollection<ProjectItem> _Children = new ObservableCollection<ProjectItem>();
        public ObservableCollection<ProjectItem> Children
        {
            get
            {
                return _Children;
            }
        }
    }
}
