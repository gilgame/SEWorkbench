using System;
using System.Xml.Serialization;

namespace Gilgame.SEWorkbench.Serialization
{
    [Serializable, XmlRoot("ProjectFile")]
    public class ProjectFile
    {
        public string ProjectName { get; set; }
    }
}
