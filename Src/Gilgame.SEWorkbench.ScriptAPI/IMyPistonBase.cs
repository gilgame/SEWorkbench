using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyPistonBase : IMyFunctionalBlock
    {
        /// <summary>
        /// Param - limit is top
        /// </summary>
        float Velocity { get; }
        float MinLimit { get; }
        float MaxLimit { get; }

        /// <summary>
        /// Gets the current position of the piston head relative to the base.
        /// </summary>
        float CurrentPosition { get; }

        /// <summary>
        /// Gets the current status.
        /// </summary>
        PistonStatus Status { get; }
    }
}
