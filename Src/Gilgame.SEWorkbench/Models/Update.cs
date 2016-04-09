using System;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Models
{
    [Serializable]
    [XmlRoot("Update")]
    public class Update
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Version")]
        public string Version { get; set; }

        [XmlElement("Location")]
        public string Location { get; set; }

        [XmlElement("Details")]
        public string Details { get; set; }

        [XmlElement("CheckSum")]
        public string CheckSum { get; set; }

        [XmlIgnore]
        public bool IsNewer { get; set; }
    }
}
