using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyGravityGenerator : IMyGravityGeneratorBase
    {
        float FieldWidth { get; }
        float FieldHeight { get; }
        float FieldDepth { get; }
        float Gravity { get; }
    }
}
