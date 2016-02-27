using System;
using System.Collections.Generic;

using VRage.Game;

namespace Gilgame.SEWorkbench.Interop
{
    public class Grid
    {
        public string Name { get; set; }

        public string Path { get; set; }

        private MyObjectBuilder_Definitions _Definitions;
        public MyObjectBuilder_Definitions Definitions
        {
            get
            {
                return _Definitions;
            }
        }

        public Grid(MyObjectBuilder_Definitions definitions)
        {
            _Definitions = definitions;
        }

        private Dictionary<string, List<Interop.TerminalBlock>> _Blocks = new Dictionary<string, List<TerminalBlock>>();
        public Dictionary<string, List<Interop.TerminalBlock>> Blocks
        {
            get
            {
                return _Blocks;
            }
            set
            {
                _Blocks = value;
            }
        }

        public void AddBlock(string type, Interop.TerminalBlock block)
        {
            if (!String.IsNullOrEmpty(type))
            {
                if (!_Blocks.ContainsKey(type))
                {
                    _Blocks.Add(type, new List<TerminalBlock>());
                }
                _Blocks[type].Add(block);
            }
        }
    }
}
