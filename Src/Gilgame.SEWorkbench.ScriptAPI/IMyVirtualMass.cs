using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyVirtualMass : IMyFunctionalBlock
    {
        /// <summary>
        /// Virtualmass weight
        /// </summary>
        float VirtualMass { get; }
    }
}
