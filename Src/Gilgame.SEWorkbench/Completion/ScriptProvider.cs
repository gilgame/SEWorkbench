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

        private List<string> _Vars = new List<string>();

        public string GetVars()
        {
            List<String> variables = new List<string>()
            {
                "public static IMyGridTerminalSystem GridTerminalSystem = null;",
            };
            variables.AddRange(_Vars);

            return String.Join(Environment.NewLine, variables.ToArray());
        }

        public void UpdateVars(List<string> vars)
        {
            _Vars.Clear();
            _Vars.AddRange(vars);
        }
    }
}
