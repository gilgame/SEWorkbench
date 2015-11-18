using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
   
    public interface IMyLandingGear : IMyFunctionalBlock
    {
        float BreakForce
        {
            get;
        }

        bool IsLocked { get; }
    }
}
