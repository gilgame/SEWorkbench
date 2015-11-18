using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyShipController : IMyTerminalBlock
    {
        /// <summary>
        /// Indicates whether a block is locally or remotely controlled.
        /// </summary>
        bool IsUnderControl { get; }
        bool ControlWheels { get; }
        bool ControlThrusters { get; }
        bool HandBrake { get; }
        bool DampenersOverride { get; }
    }
}
