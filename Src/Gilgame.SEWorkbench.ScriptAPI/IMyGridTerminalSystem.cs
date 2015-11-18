using System;
using System.Collections.Generic;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyGridTerminalSystem
    {
        void GetBlocks(List<IMyTerminalBlock> blocks);
        void GetBlockGroups(List<IMyBlockGroup> blockGroups);
        void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null);
        void SearchBlocksOfName(string name,List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null);
        IMyTerminalBlock GetBlockWithName(string name);
    }
}
