using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Models
{
    [Serializable]
    [XmlRoot("Item")]
    public class ProjectItem
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Type")]
        public ProjectItemType Type { get; set; }

        [XmlElement("Path")]
        public string Path { get; set; }

        [XmlIgnore]
        public ViewModels.ProjectViewModel Project { get; set; }

        private ObservableCollection<ProjectItem> _Children = new ObservableCollection<ProjectItem>();

        [XmlElement("Children")]
        public ObservableCollection<ProjectItem> Children
        {
            get
            {
                return _Children;
            }
            set
            {
                _Children = value;
            }
        }
    }
}
