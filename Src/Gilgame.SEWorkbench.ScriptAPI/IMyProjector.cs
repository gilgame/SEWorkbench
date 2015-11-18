using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyProjector : IMyFunctionalBlock
    {
        int ProjectionOffsetX { get; }
        int ProjectionOffsetY { get; }
        int ProjectionOffsetZ { get; }

        int ProjectionRotX { get; }
        int ProjectionRotY { get; }
        int ProjectionRotZ { get; }

        int RemainingBlocks { get; }

        void LoadRandomBlueprint(string searchPattern);
        void LoadBlueprint(string name);
    }
}
