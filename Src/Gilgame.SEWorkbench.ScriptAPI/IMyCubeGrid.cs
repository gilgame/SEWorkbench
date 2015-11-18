using System;
using System.Collections.Generic;
namespace Gilgame.SEWorkbench.ScriptAPI
{
    /// <summary>
    /// Grid interface
    /// </summary>
    public interface IMyCubeGrid : IMyEntity
    {
        /// <summary>
        /// Grid size in meters
        /// </summary>
        float GridSize { get; }


        /// <summary>
        /// Station = static
        /// </summary>
        bool IsStatic { get; }
    }
}
