using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyGyro : IMyFunctionalBlock
    {
        float GyroPower { get; }
        bool GyroOverride { get; }
        float Yaw { get; }
        float Pitch { get; }
        float Roll { get; }
    }
}
