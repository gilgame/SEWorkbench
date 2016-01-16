using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Medieval.ObjectBuilders;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Interfaces;
using VRage;
using VRage.Compiler;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;


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
            Sandbox.Game.Gui.MyGuiScreenEditor.CompileProgram(program, _CompileErrors, ref temp);
        }

        public static void Init()
        {
            InitIlChecker();
            InitIlCompiler();

            _Initialized = true;
        }

        private static void InitIlChecker()
        {
            IlChecker.AllowNamespaceOfTypeCommon(typeof(TerminalActionExtensions));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.ModAPI.Ingame.IMyCubeBlock));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(EnvironmentItemsEntry));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AdvancedDoor));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_BattleAreaMarker));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_HandTorch));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AdvancedDoorDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_Base));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AirVent));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Vector3));
            Type typeFromHandle = typeof(MyObjectBuilderSerializer);
            IlChecker.AllowedOperands[typeFromHandle] = new List<MemberInfo>
	        {
		        typeFromHandle.GetMethod("CreateNewObject", new Type[]
		        {
			        typeof(MyObjectBuilderType)
		        }),
		        typeFromHandle.GetMethod("CreateNewObject", new Type[]
		        {
			        typeof(SerializableDefinitionId)
		        }),
		        typeFromHandle.GetMethod("CreateNewObject", new Type[]
		        {
			        typeof(string)
		        }),
		        typeFromHandle.GetMethod("CreateNewObject", new Type[]
		        {
			        typeof(MyObjectBuilderType),
			        typeof(string)
		        })
	        };
                    IlChecker.AllowedOperands.Add(typeof(IMyEntity), new List<MemberInfo>
	        {
		        typeof(IMyEntity).GetMethod("GetPosition"),
		        typeof(IMyEntity).GetProperty("WorldMatrix").GetGetMethod()
	        });
            IlChecker.AllowedOperands.Add(typeof(IWork), null);
            IlChecker.AllowedOperands.Add(typeof(Task), null);
            IlChecker.AllowedOperands.Add(typeof(WorkOptions), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.ITerminalAction), null);
            IlChecker.AllowedOperands.Add(typeof(IMyInventoryOwner), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.IMyInventory), null);
            IlChecker.AllowedOperands.Add(typeof(IMyInventoryItem), null);
            IlChecker.AllowedOperands.Add(typeof(ITerminalProperty), null);
            IlChecker.AllowedOperands.Add(typeof(ITerminalProperty<>), null);
            IlChecker.AllowedOperands.Add(typeof(TerminalPropertyExtensions), null);
            IlChecker.AllowedOperands.Add(typeof(MyFixedPoint), null);
            IlChecker.AllowedOperands.Add(typeof(MyTexts), null);
        }

        private static void InitIlCompiler()
        {
            Func<string, string> func = (string x) => Path.Combine(VRage.FileSystem.MyFileSystem.ExePath, x);
            IlCompiler.Options = new System.CodeDom.Compiler.CompilerParameters(new string[]
			{
				func("SpaceEngineers.ObjectBuilders.dll"),
				func("MedievalEngineers.ObjectBuilders.dll"),
				func("Sandbox.Game.dll"),
				func("Sandbox.Common.dll"),
				func("Sandbox.Graphics.dll"),
				func("VRage.dll"),
				func("VRage.Library.dll"),
				func("VRage.Math.dll"),
				func("VRage.Game.dll"),
				"System.Xml.dll",
				"System.Core.dll",
				"System.dll"
			});
        }
    }
}
