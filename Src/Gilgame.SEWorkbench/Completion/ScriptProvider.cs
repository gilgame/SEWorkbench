using System;
using System.Collections.Generic;

using ICSharpCode.CodeCompletion;

namespace Gilgame.SEWorkbench.Completion
{
    public class ScriptProvider : ICSharpScriptProvider
    {
        public string GetUsing()
        {
            List<String> usings = new List<string>()
            {
                "using System;",
                "using System.Collections.Generic;",
                "using System.Linq;",
                "using System.Text;",
                "using Sandbox.ModAPI.Ingame;",
                "using Sandbox.ModAPI.Interfaces;",
                "using VRageMath;",
                "using VRage.Game;",
            };

            return String.Join(Environment.NewLine, usings.ToArray());
        }

        public string GetVars()
        {
            List<String> variables = new List<string>()
            {
                "static IMyGridTerminalSystem GridTerminalSystem = new Gilgame.SEWorkbench.Interop.GridTerminalSystem();",
            };

            return String.Join(Environment.NewLine, variables.ToArray());
        }
    }
}
