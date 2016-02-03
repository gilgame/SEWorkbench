using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Medieval.ObjectBuilders;
using Medieval.ObjectBuilders.Definitions;
using ParallelTasks;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Common.ObjectBuilders.Voxels;
using Sandbox.Common.ObjectBuilders.VRageData;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage;
using VRage.Collections;
using VRage.Compiler;
using VRage.FileSystem;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Gilgame.SEWorkbench.Interop
{
    public class SpaceEngineers
    {
        private static bool _Initialized = false;

        public static void Initialize()
        {
            if (_Initialized)
            {
                return;
            }

            SetPaths();

            #if DEBUG
                EnableLogging();
            #endif

            RegisterPlugins();
            InitIlChecker();
            InitIlCompiler();

            _Initialized = true;
        }

        private static void SetPaths()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sepath = Path.Combine(appdata, "SpaceEngineers");

            MyFileSystem.Init(sepath, sepath);
        }

        private static void RegisterPlugins()
        {
            MyPlugins.RegisterGameAssemblyFile("SpaceEngineers.Game.dll");
            MyPlugins.RegisterGameObjectBuildersAssemblyFile("SpaceEngineers.ObjectBuilders.dll");
            MyPlugins.RegisterSandboxAssemblyFile("Sandbox.Common.dll");
            MyPlugins.RegisterSandboxGameAssemblyFile("Sandbox.Game.dll");
            MyPlugins.Load();

            MyObjectBuilderType.RegisterAssemblies();
            MyObjectBuilderSerializer.RegisterAssembliesAndLoadSerializers();
        }

        private static void EnableLogging()
        {
            MyLog.Default = new MyLog();
            MyLog.Default.Init("test.log", new System.Text.StringBuilder());
        }

        private static void InitIlChecker()
        {
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_SessionSettings));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(TerminalActionExtensions));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(SerializableBlockOrientation));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.ModAPI.Ingame.IMyCubeBlock));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(IMySession));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(IMyCameraController));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(IMyEntity));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(IMyEntities));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyEntity));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyEntityExtensions));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(EnvironmentItemsEntry));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyIngameScript));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyGameLogicComponent));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(IMyComponentBase));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MySessionComponentBase));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_AdvancedDoor));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_BattleAreaMarker));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AdvancedDoor));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_BattleAreaMarker));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_HandTorch));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_HandTorch));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_AdvancedDoorDefinition));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_BarbarianWaveEventDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AdvancedDoorDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_BarbarianWaveEventDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_Base));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AirVent));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_VoxelMap));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyStatLogic));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_EntityStatRegenEffect));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyEntityStat));
            IlChecker.AllowedOperands.Add(typeof(MyCharacterMovement), null);
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(SerializableDefinitionId));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(SerializableVector3));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyDefinitionId));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyDefinitionManager));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Vector3));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyFixedPoint));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(ListReader<>));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyStorageData));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyEventArgs));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyGameTimer));
            Type typeFromHandle = typeof(MyObjectBuilderSerializer);
            IlChecker.AllowedOperands[typeFromHandle] = new List<MemberInfo>
	        {
		        typeFromHandle.GetMethod("CreateNewObject", new Type[] { typeof(MyObjectBuilderType) }),
		        typeFromHandle.GetMethod("CreateNewObject", new Type[] { typeof(SerializableDefinitionId) }),
		        typeFromHandle.GetMethod("CreateNewObject", new Type[] { typeof(string) }),
		        typeFromHandle.GetMethod("CreateNewObject", new Type[] { typeof(MyObjectBuilderType), typeof(string) })
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
            Func<string, string> func = (string x) => Path.Combine(MyFileSystem.ExePath, x);
            IlCompiler.Options = new CompilerParameters(new string[]
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
