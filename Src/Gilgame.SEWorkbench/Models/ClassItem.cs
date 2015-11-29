using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Models
{
    [Serializable]
    [XmlRoot("ClassItem")]
    public class ClassItem
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Namespace")]
        public string Namespace { get; set; }

        [XmlElement("Type")]
        public ClassItemType Type { get; set; }

        private ObservableCollection<ClassItem> _Children = new ObservableCollection<ClassItem>();

        [XmlElement("Items")]
        public ObservableCollection<ClassItem> Children
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
