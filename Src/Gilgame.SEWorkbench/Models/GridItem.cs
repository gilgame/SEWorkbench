using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Models
{
    [Serializable]
    [XmlRoot("GridItem")]
    public class GridItem
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("EntityID")]
        public long EntityID { get; set; }

        [XmlElement("Program")]
        public string Program { get; set; }

        [XmlElement("Type")]
        public GridItemType Type { get; set; }

        private ObservableCollection<GridItem> _Children = new ObservableCollection<GridItem>();

        [XmlElement("Items")]
        public ObservableCollection<GridItem> Children
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
