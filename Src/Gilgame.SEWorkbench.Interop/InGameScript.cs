using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

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

        private static bool CompileProgram(string script, List<string> errors, ref Assembly assembly)
        {
            if (script != null && script.Length > 0)
            {
                ScriptProvider provider = new ScriptProvider();

                StringBuilder program = new StringBuilder();
                program.AppendLine(provider.GetUsing());
                program.AppendLine("public class Program : MyGridProgram {");
                program.AppendLine(script);
                program.AppendLine("}");
                
                if (VRage.Compiler.IlCompiler.CompileStringIngame(Path.Combine(VRage.FileSystem.MyFileSystem.UserDataPath, "IngameScript.dll"), new string[]
		        {
			        program.ToString()
		        },
                out assembly, errors))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
