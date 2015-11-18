using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyBatteryBlock : IMyFunctionalBlock
    {
        bool HasCapacityRemaining { get; }

        float CurrentStoredPower { get; }
        float MaxStoredPower { get; }
    }
}
