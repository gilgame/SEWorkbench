using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMySmallMissileLauncher : IMyUserControllableGun
    {
        bool UseConveyorSystem { get; }
    }
}
