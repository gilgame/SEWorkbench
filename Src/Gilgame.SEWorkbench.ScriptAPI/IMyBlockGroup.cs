﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public interface IMyBlockGroup
    {
        List<IMyTerminalBlock> Blocks { get; }
        String Name { get;}
    }
}
