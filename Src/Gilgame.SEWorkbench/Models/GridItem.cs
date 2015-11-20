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
