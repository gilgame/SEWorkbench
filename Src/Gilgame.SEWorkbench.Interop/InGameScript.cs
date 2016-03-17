using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Gilgame.SEWorkbench.Interop
{
    public class InGameScript
    {
        private List<String> _CompileErrors = new List<string>();
        public List<String> CompileErrors
        {
            get
            {
                return _CompileErrors;
            }
        }

        public InGameScript(string program)
        {
            Assembly temp = null;
            CompileProgram(program, _CompileErrors, ref temp);
        }

        private static bool CompileProgram(string program, List<string> errors, ref Assembly assembly)
        {
            if (program != null && program.Length > 0)
            {
                string text = "using System;\nusing System.Collections.Generic;\nusing VRageMath;\nusing VRage.Game;\nusing VRage.ModAPI.Ingame;\nusing System.Text;\nusing Sandbox.ModAPI.Interfaces;\nusing Sandbox.ModAPI.Ingame;\nusing Sandbox.Game.EntityComponents;\nusing VRage.Game.Components;\nusing VRage.Collections;\nusing VRage.Game.ObjectBuilders.Definitions;\npublic class Program: MyGridProgram\n{\n" + program + "\n}";
                if (VRage.Compiler.IlCompiler.CompileStringIngame(Path.Combine(VRage.FileSystem.MyFileSystem.UserDataPath, "IngameScript.dll"), new string[]
		        {
			        text
		        }, out assembly, errors))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
