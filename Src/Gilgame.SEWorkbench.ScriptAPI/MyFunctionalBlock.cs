using System;

using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public class MyFunctionalBlock : MyTerminalBlock, IMyFunctionalBlock
    {
        public bool Enabled { get; set; }

        public void RequestEnable(bool enable)
        {
            throw new NotImplementedException();
        }
    }
}
