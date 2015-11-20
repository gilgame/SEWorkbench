using System;

using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public class MyAssembler : MyProductionBlock, IMyAssembler
    {
        public bool DisassembleEnabled { get; set; }
    }
}
