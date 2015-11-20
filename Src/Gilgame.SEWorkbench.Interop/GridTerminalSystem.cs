using System;
using System.Collections.Generic;
using System.Linq;

using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.Interop
{
    public class GridTerminalSystem : IMyGridTerminalSystem
    {
        private List<IMyTerminalBlock> _Blocks = new List<IMyTerminalBlock>();
        private List<IMyBlockGroup> _BlockGroups = new List<IMyBlockGroup>();

        internal void AddBlock(IMyTerminalBlock block)
        {
            if (block != null)
            {
                _Blocks.Add(block);
            }
        }

        internal void AddRange(List<IMyTerminalBlock> blocks)
        {
            _Blocks.AddRange(blocks);
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks)
        {
            blocks.Clear();
            foreach (IMyTerminalBlock block in _Blocks)
            {
                blocks.Add(block);
            }
        }

        public void GetBlockGroups(List<IMyBlockGroup> blockGroups)
        {
            blockGroups.Clear();
            blockGroups.AddRange(_BlockGroups);
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            blocks.Clear();
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

        public void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            blocks.Clear();
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
    }
}
