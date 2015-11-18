using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyShipConnector:IMyFunctionalBlock
    {
        bool ThrowOut { get; }
        bool CollectAll { get; }
        bool IsLocked { get; }
        bool IsConnected { get; }
        IMyShipConnector OtherConnector { get; }
    }
}
