using System;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Models
{
    [Serializable]
    [XmlRoot("Project")]
    public class Project
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Path")]
        public string Path { get; set; }

        [XmlElement("RootItem")]
        public ProjectItem RootItem { get; set; }

        [XmlElement("OpenState")]
        public bool OpenState { get; set; }
    }
}
