using System;
using System.Collections.Generic;

using ICSharpCode.CodeCompletion;

namespace Gilgame.SEWorkbench.Interop
{
    public class ScriptProvider : ICSharpScriptProvider
    {
        public string GetUsing()
        {
            List<String> usings = new List<string>()
            {
                "using System;",
                "using System.Collections.Generic;",
                "using VRageMath;",
                "using VRage.Game;",
                "using VRage.ModAPI.Ingame;",
                "using System.Text;",
                "using Sandbox.ModAPI.Interfaces;",
                "using Sandbox.ModAPI.Ingame;",
                "using VRage.Game.ModAPI.Ingame;",
                "using Sandbox.Game.EntityComponents;",
                "using VRage.Game.Components;",
                "using VRage.Collections;",
                "using VRage.Game.ObjectBuilders.Definitions;",
                "using SpaceEngineers.Game.ModAPI.Ingame;",
            };

            return String.Join(Environment.NewLine, usings.ToArray());
        }

        private List<string> _Vars = new List<string>();

        public string GetVars()
        {
            List<String> variables = new List<string>()
            {
                //"public static IMyGridTerminalSystem GridTerminalSystem = null;",
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
