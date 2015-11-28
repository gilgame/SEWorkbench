using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Sandbox.Game.Gui;
using VRage.Compiler;
using VRage.FileSystem;

namespace Gilgame.SEWorkbench.Interop
{
    public class InGameScript
    {
        private static bool _Initialized = false;

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
            if (!_Initialized)
            {
                Init();
            }

            Assembly temp = null;
            MyGuiScreenEditor.CompileProgram(program, _CompileErrors, ref temp);
        }

        public static void Init()
        {
            InitIlChecker();
            InitIlCompiler();

            _Initialized = true;
        }

        private static void InitIlChecker()
        {
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.ModAPI.Ingame.IMyCubeBlock));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.ModAPI.Interfaces.ITerminalAction));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_AirVent));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Common.ObjectBuilders.Definitions.EnvironmentItemsEntry));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(VRage.MyFixedPoint));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(VRage.ModAPI.IMyEntity));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(VRage.ObjectBuilders.MyObjectBuilder_Base));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(VRageMath.Vector3));
        }

        private static void InitIlCompiler()
        {
            Func<string, string> getPath = (x) => Path.Combine(MyFileSystem.ExePath, x);
            IlCompiler.Options = new System.CodeDom.Compiler.CompilerParameters(
                new string[] {
                    "System.dll",
                    "System.Xml.dll",
                    "System.Core.dll",
                    getPath("Sandbox.Game.dll"),
                    getPath("Sandbox.Common.dll"),
                    getPath("Sandbox.Graphics.dll"),
                    getPath("VRage.dll"),
                    getPath("VRage.Library.dll"),
                    getPath("VRage.Math.dll"),
                    getPath("VRage.Game.dll")
                }
            );
        }
    }
}
