using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VRage.Compiler;
using VRage.ModAPI;


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
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_SessionSettings));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Game.Gui.TerminalActionExtensions));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.ObjectBuilders.VRageData.SerializableBlockOrientation));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.ModAPI.Ingame.IMyCubeBlock));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.ModAPI.IMySession));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.ModAPI.Interfaces.IMyCameraController));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.ModAPI.IMyEntity));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Common.ObjectBuilders.Definitions.EnvironmentItemsEntry));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.Components.MyGameLogicComponent));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.Components.IMyComponentBase));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.MySessionComponentBase));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_AdvancedDoor));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_BattleAreaMarker));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_AdvancedDoor));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_BattleAreaMarker));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Medieval.ObjectBuilders.MyObjectBuilder_HandTorch));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Medieval.ObjectBuilders.MyObjectBuilder_HandTorch));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.ObjectBuilders.Definitions.MyObjectBuilder_AdvancedDoorDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Common.ObjectBuilders.Definitions.MyObjectBuilder_AdvancedDoorDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(VRage.ObjectBuilders.MyObjectBuilder_Base));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_AirVent));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Common.ObjectBuilders.Voxels.MyObjectBuilder_VoxelMap));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Game.MyStatLogic));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.Game.ObjectBuilders.MyObjectBuilder_EntityStatRegenEffect));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Game.Entities.MyEntityStat));
            IlChecker.AllowedOperands.Add(typeof(Sandbox.Common.ObjectBuilders.MyCharacterMovement), null);
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.ObjectBuilders.SerializableDefinitionId));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.SerializableVector3));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Definitions.MyDefinitionId));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(Sandbox.Definitions.MyDefinitionManager));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(VRageMath.Vector3));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.MyFixedPoint));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.Collections.ListReader<>));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.Voxels.MyStorageData));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.Utils.MyEventArgs));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.Library.Utils.MyGameTimer));
            Type typeFromHandle = typeof(VRage.ObjectBuilders.MyObjectBuilderSerializer);
            IlChecker.AllowedOperands[typeFromHandle] = new List<MemberInfo>
			{
				typeFromHandle.GetMethod("CreateNewObject", new Type[]
				{
					typeof(VRage.ObjectBuilders.MyObjectBuilderType)
				}),
				typeFromHandle.GetMethod("CreateNewObject", new Type[]
				{
					typeof(VRage.ObjectBuilders.SerializableDefinitionId)
				}),
				typeFromHandle.GetMethod("CreateNewObject", new Type[]
				{
					typeof(string)
				}),
				typeFromHandle.GetMethod("CreateNewObject", new Type[]
				{
					typeof(VRage.ObjectBuilders.MyObjectBuilderType),
					typeof(string)
				})
			};
            IlChecker.AllowedOperands.Add(typeof(IMyEntity), new List<MemberInfo>
			{
				typeof(IMyEntity).GetMethod("GetPosition"),
				typeof(IMyEntity).GetProperty("WorldMatrix").GetGetMethod()
			});
            IlChecker.AllowedOperands.Add(typeof(ParallelTasks.IWork), null);
            IlChecker.AllowedOperands.Add(typeof(ParallelTasks.Task), null);
            IlChecker.AllowedOperands.Add(typeof(ParallelTasks.WorkOptions), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.ITerminalAction), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.IMyInventoryOwner), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.IMyInventory), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.IMyInventoryItem), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.ITerminalProperty), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.ITerminalProperty<>), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.TerminalPropertyExtensions), null);
            IlChecker.AllowedOperands.Add(typeof(VRage.MyFixedPoint), null);
            IlChecker.AllowedOperands.Add(typeof(VRage.MyTexts), null);
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
