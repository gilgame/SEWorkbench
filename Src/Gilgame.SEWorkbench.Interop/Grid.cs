using Sandbox.Common.ObjectBuilders.Definitions;
using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.Interop
{
    public class Grid
    {
        public string Name { get; set; }

        public string Path { get; set; }

        private MyObjectBuilder_ShipBlueprintDefinition _Definition;
        public MyObjectBuilder_ShipBlueprintDefinition Definition
        {
            get
            {
                return _Definition;
            }
        }

        public Grid(MyObjectBuilder_ShipBlueprintDefinition definition)
        {
            _Definition = definition;
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
