using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    /// <summary>
    /// Antenna block interface
    /// </summary>
    public interface IMyRadioAntenna : IMyFunctionalBlock
    {
        /// <summary>
        /// Broadcasting/Receiving range (read-only)
        /// </summary>
        float Radius {get; }

        /// <summary>
        /// Show shipname on hud (read-only)
        /// </summary>
        bool ShowShipName { get; }

        /// <summary>
        /// Returns true if antena is broadcasting (read-only)
        /// </summary>
        bool IsBroadcasting { get; }
	}
}
