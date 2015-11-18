using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyRemoteControl : IMyShipController
    {
        void ClearWaypoints();
        void SetAutoPilotEnabled(bool enabled);

    }
}
