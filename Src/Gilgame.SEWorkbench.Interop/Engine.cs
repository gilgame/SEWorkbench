using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage;
using VRage.Collections;
using VRage.Compiler;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Gilgame.SEWorkbench.Interop
{
    public class Engine
    {
        public static List<string> Dependencies
        {
            get
            {
                List<string> assemblies = new List<string>()
                {
                    "HavokWrapper.dll",
                    "InfinarioSDK.dll",
                    "Sandbox.Common.dll",
                    "Sandbox.Game.dll",
                    "Sandbox.Graphics.dll",
                    "SharpDX.Direct2D1.dll",
                    "SharpDX.Direct3D11.dll",
                    "SharpDX.DirectInput.dll",
                    "SharpDX.dll",
                    "SharpDX.DXGI.dll",
                    "SharpDX.Toolkit.dll",
                    "SharpDX.Toolkit.Graphics.dll",
                    "SharpDX.XAudio2.dll",
                    "SteamSDK.dll",
                    "steam_api.dll",
                    "System.Data.SQLite.dll",
                    "VRage.Audio.dll",
                    "VRage.dll",
                    "VRage.Game.dll",
                    "VRage.Game.XmlSerializers.dll",
                    "VRage.Input.dll",
                    "VRage.Library.dll",
                    "VRage.Math.dll",
                    "VRage.Native.dll",
                    "SpaceEngineers.Game.dll",
                    "SpaceEngineers.ObjectBuilders.dll",
                    "SpaceEngineers.ObjectBuilders.XmlSerializers.dll",
                };
                return assemblies;
            }
        }

        private static bool _Initialized = false;
        public static bool Initialized
        {
            get
            {
                return _Initialized;
            }
        }
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
            LoadSerializers();
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

        private static void LoadSerializers()
        {
            MyObjectBuilder_Base loaded = null;

            try { MyObjectBuilderSerializer.DeserializeXML(String.Empty, out loaded); }
            catch { }
        }

        private static void InitIlChecker()
        {
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(SpaceEngineers.Game.ModAPI.IMyButtonPanel));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel));

            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyFactionMember));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyFontEnum));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_SessionSettings));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(TerminalActionExtensions));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(SerializableBlockOrientation));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(VRage.Game.ModAPI.Ingame.IMyCubeBlock));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Sandbox.ModAPI.Ingame.IMyTerminalBlock));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.Game.ModAPI.IMyCubeBlock));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyFinalBuildConstants));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyAPIGateway));
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
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AdvancedDoor));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_AdvancedDoorDefinition));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyObjectBuilder_BarbarianWaveEventDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_AdvancedDoorDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_BarbarianWaveEventDefinition));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyObjectBuilder_Base));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(MyDefinitionBase));
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
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyDefinitionManagerBase));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(Vector3));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyFixedPoint));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(ListReader<>));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyStorageData));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyEventArgs));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyGameTimer));
            IlChecker.AllowNamespaceOfTypeCommon(typeof(IMyInventoryItem));
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(MyLight));
            Type typeFromHandle = typeof(MyObjectBuilderSerializer);
            IlChecker.AllowedOperands[typeFromHandle] = new HashSet<MemberInfo>
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
            IlChecker.AllowedOperands.Add(typeof(IMyEntity), new HashSet<MemberInfo>
            {
                typeof(IMyEntity).GetMethod("GetPosition"),
                typeof(IMyEntity).GetProperty("WorldMatrix").GetGetMethod(),
                typeof(IMyEntity).GetProperty("Components").GetGetMethod()
            });
            IlChecker.AllowedOperands.Add(typeof(IWork), null);
            IlChecker.AllowedOperands.Add(typeof(Task), null);
            IlChecker.AllowedOperands.Add(typeof(WorkOptions), null);
            IlChecker.AllowedOperands.Add(typeof(Sandbox.ModAPI.Interfaces.ITerminalAction), null);
            IlChecker.AllowedOperands.Add(typeof(IMyInventoryOwner), null);
            IlChecker.AllowedOperands.Add(typeof(VRage.Game.ModAPI.Ingame.IMyInventory), null);
            IlChecker.AllowedOperands.Add(typeof(IMyInventoryItem), null);
            IlChecker.AllowedOperands.Add(typeof(ITerminalProperty), null);
            IlChecker.AllowedOperands.Add(typeof(ITerminalProperty<>), null);
            IlChecker.AllowedOperands.Add(typeof(TerminalPropertyExtensions), null);
            IlChecker.AllowedOperands.Add(typeof(MyFixedPoint), null);
            IlChecker.AllowedOperands.Add(typeof(MyTexts), null);
            IlChecker.AllowNamespaceOfTypeModAPI(typeof(VRage.ModAPI.IMyInput));
            IlChecker.AllowedOperands.Add(typeof(MyInputExtensions), null);
            IlChecker.AllowedOperands.Add(typeof(MyKeys), null);
            IlChecker.AllowedOperands.Add(typeof(MyJoystickAxesEnum), null);
            IlChecker.AllowedOperands.Add(typeof(MyJoystickButtonsEnum), null);
            IlChecker.AllowedOperands.Add(typeof(MyMouseButtonsEnum), null);
            IlChecker.AllowedOperands.Add(typeof(MySharedButtonsEnum), null);
            IlChecker.AllowedOperands.Add(typeof(MyGuiControlTypeEnum), null);
            IlChecker.AllowedOperands.Add(typeof(MyGuiInputDeviceEnum), null);
            IEnumerable<MethodInfo> source = from method in typeof(MyComponentContainer).GetMethods()
                                             where method.Name == "TryGet" && method.ContainsGenericParameters && method.GetParameters().Length == 1
                                             select method;
            IlChecker.AllowedOperands.Add(typeof(MyComponentContainer), new HashSet<MemberInfo>
            {
                typeof(MyComponentContainer).GetMethod("Has").MakeGenericMethod(new Type[]
                {
                    typeof(MyResourceSourceComponent)
                }),
                typeof(MyComponentContainer).GetMethod("Get").MakeGenericMethod(new Type[]
                {
                    typeof(MyResourceSourceComponent)
                }),
                typeof(MyComponentContainer).GetMethod("TryGet", new Type[]
                {
                    typeof(Type),
                    typeof(MyResourceSourceComponent)
                }),
                source.FirstOrDefault<MethodInfo>().MakeGenericMethod(new Type[]
                {
                    typeof(MyResourceSourceComponent)
                }),
                typeof(MyComponentContainer).GetMethod("Has").MakeGenericMethod(new Type[]
                {
                    typeof(MyResourceSinkComponent)
                }),
                typeof(MyComponentContainer).GetMethod("Get").MakeGenericMethod(new Type[]
                {
                    typeof(MyResourceSinkComponent)
                }),
                typeof(MyComponentContainer).GetMethod("TryGet", new Type[]
                {
                    typeof(Type),
                    typeof(MyResourceSinkComponent)
                }),
                source.FirstOrDefault<MethodInfo>().MakeGenericMethod(new Type[]
                {
                    typeof(MyResourceSinkComponent)
                })
            });
            IlChecker.AllowedOperands.Add(typeof(MyResourceSourceComponentBase), null);
            IlChecker.AllowedOperands.Add(typeof(MyResourceSinkComponentBase), new HashSet<MemberInfo>
            {
                typeof(MyResourceSinkComponentBase).GetProperty("AcceptedResources").GetGetMethod(),
                typeof(MyResourceSinkComponentBase).GetMethod("CurrentInputByType"),
                typeof(MyResourceSinkComponentBase).GetMethod("IsPowerAvailable"),
                typeof(MyResourceSinkComponentBase).GetMethod("IsPoweredByType"),
                typeof(MyResourceSinkComponentBase).GetMethod("MaxRequiredInputByType"),
                typeof(MyResourceSinkComponentBase).GetMethod("RequiredInputByType"),
                typeof(MyResourceSinkComponentBase).GetMethod("SuppliedRatioByType")
            });
            IlChecker.AllowedOperands.Add(typeof(ListReader<MyDefinitionId>), null);
            IlChecker.AllowedOperands.Add(typeof(MyDefinitionId), null);
        }

        private static void InitIlCompiler()
        {
            Func<string, string> func = (string x) => Path.Combine(MyFileSystem.ExePath, x);
            IlCompiler.Options = new CompilerParameters(new string[]
	        {
			    func("SpaceEngineers.ObjectBuilders.dll"),
			    func("SpaceEngineers.Game.dll"),
			    func("Sandbox.Game.dll"),
			    func("Sandbox.Common.dll"),
			    func("Sandbox.Graphics.dll"),
			    func("VRage.dll"),
			    func("VRage.Library.dll"),
			    func("VRage.Math.dll"),
			    func("VRage.Game.dll"),
			    "System.Core.dll",
			    "System.Xml.dll",
			    "System.dll"
	        });
            IlCompiler.Options.GenerateInMemory = true;
            IlCompiler.Options.CompilerOptions = String.Format("/debug {0}", IlCompiler.Options.CompilerOptions);
        }
    }
}
