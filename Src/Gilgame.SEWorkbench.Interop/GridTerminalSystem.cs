using System;
using System.Collections.Generic;
using System.Linq;

using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.Interop
{
    /// <summary>
    /// Basic class for implementing IMyGridTerminalSystem
    /// </summary>
    public class GridTerminalSystem : IMyGridTerminalSystem
    {
        private List<IMyTerminalBlock> _Blocks = new List<IMyTerminalBlock>();
        List<IMyBlockGroup> _BlockGroups = new List<IMyBlockGroup>();

        internal void AddBlock(IMyTerminalBlock block)
        {
            _Blocks.Add(block);
        }

        public void GetBlockGroups(List<IMyBlockGroup> blockGroups)
        {
            // TODO add block groups support
            throw new NotImplementedException();
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks)
        {
            blocks.AddRange(_Blocks);
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            foreach (IMyTerminalBlock block in _Blocks)
            {
                if (block is T)
                {
                    if (collect == null || collect.Invoke(block))
                    {
                        blocks.Add(block);
                    }
                }
            }
        }

        public IMyTerminalBlock GetBlockWithName(string name)
        {
            foreach (IMyTerminalBlock block in _Blocks)
            {
                if (block.CustomName == name)
                {
                    return block;
                }
            }
            return null;
        }

        public void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            foreach (IMyTerminalBlock block in _Blocks)
            {
                if (block.CustomName.ToLower().IndexOf(name.ToLower()) > -1)
                {
                    if (collect == null || collect.Invoke(block))
                    {
                        blocks.Add(block);
                    }
                }
            }
        }
    }
}
