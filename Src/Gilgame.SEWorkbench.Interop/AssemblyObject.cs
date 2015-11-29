using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.Interop
{
    public class AssemblyObject
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        private List<AssemblyObject> _Fields = new List<AssemblyObject>();
        public List<AssemblyObject> Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
            }
        }

        private List<AssemblyObject> _Properties = new List<AssemblyObject>();
        public List<AssemblyObject> Properties
        {
            get
            {
                return _Properties;
            }
            set
            {
                _Properties = value;
            }
        }

        private List<AssemblyObject> _Methods = new List<AssemblyObject>();
        public List<AssemblyObject> Methods
        {
            get
            {
                return _Methods;
            }
            set
            {
                _Methods = value;
            }
        }

        private List<AssemblyObject> _Children = new List<AssemblyObject>();
        public List<AssemblyObject> Children
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
