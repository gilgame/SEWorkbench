using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyFunctionalBlock : IMyTerminalBlock
    {
        bool Enabled { get; }
        void RequestEnable(bool enable);
    }
}
