using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Models
{
    [Serializable]
    [XmlRoot("Project")]
    public class Project
    {
        // file missing?
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Path")]
        public string Path { get; set; }

        [XmlElement("RootItem")]
        public ProjectItem RootItem { get; set; }
    }
}
