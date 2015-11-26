using System;

namespace Gilgame.SEWorkbench.Interop
{
    public class TerminalBlock
    {
        public string Name { get; set; }

        public long EntityID { get; set; }

        public bool IsProgram { get; set; }

        public string Program { get; set; }
    }
}
