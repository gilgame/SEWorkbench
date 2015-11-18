using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyLightingBlock : IMyFunctionalBlock
    {
        float Radius{ get;}
        float Intensity{get;}
        float BlinkIntervalSeconds{get;}
        float BlinkLenght{get;}
        float BlinkOffset{get;}
    }
}
