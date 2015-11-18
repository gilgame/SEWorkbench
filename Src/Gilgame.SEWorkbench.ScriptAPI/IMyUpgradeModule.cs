using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    /// <summary>
    /// Interface to access module upgrades properties
    /// </summary>
    public interface IMyUpgradeModule
    {
        /// <summary>
        /// Retrieve number of upgrade effects this block has (r/o)
        /// </summary>
        uint UpgradeCount { get; }
        /// <summary>
        /// Retrieve number of blocks this block is connected to (r/o)
        /// </summary>
        uint Connections { get; }
    }
}
