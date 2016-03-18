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

        private static ScriptProvider _Provider;
        public static ScriptProvider Provider
        {
            get
            {
                if (_Provider == null)
                {
                    _Provider = new ScriptProvider();
                }
                return _Provider;
            }
        }

        public static int HeaderSize
        {
            get
            {
                return Header.Split(new char[] { '\n' }).Length;
            }
        }

        public static string Header
        {
            get
            {
                StringBuilder header = new StringBuilder();
                header.AppendLine(Provider.GetUsing());
                header.AppendLine("public class Program : MyGridProgram {");
                header.AppendLine(Provider.GetVars());

                return header.ToString();
            }
        }

        public static string Footer
        {
            get
            {
                return "}";
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
                program.AppendLine(Header);
                program.AppendLine(script);
                program.AppendLine(Footer);

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
