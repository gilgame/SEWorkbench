using System;

namespace Gilgame.SEWorkbench.Models
{
    public class MenuItem
    {
        public string Name { get; set; }

        public string Header { get; set; }

        public string InputGestureText { get; set; }

        public Guid Identifier { get; set; }
    }
}
