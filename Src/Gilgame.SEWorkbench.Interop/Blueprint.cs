using System;
using System.Text;

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Gui;
using Sandbox.ModAPI.Ingame;

namespace Gilgame.SEWorkbench.Interop
{
    /// <summary>
    /// Imports blueprint from file and converts to GridTerminalSystem
    /// </summary>
    public class Blueprint
    {
        private MyObjectBuilder_ShipBlueprintDefinition _Blueprint;
        private bool IsLoaded
        {
            get
            {
                return (_Blueprint != null);
            }
        }

        private GridTerminalSystem _GridTerminalSystem;
        public GridTerminalSystem GridTerminalSystem
        {
            get
            {
                return _GridTerminalSystem;
            }
        }

        private StringBuilder _Buffer;
        private StringBuilder CreateBuffer()
        {
            _Buffer = new StringBuilder();
            return _Buffer;
        }

        public void Import(string filename)
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sepath = String.Format("{0}\\SpaceEngineers", appdata);

            // load init, this should probably happen somewhere else
            VRage.FileSystem.MyFileSystem.Init(sepath, sepath);

            MyObjectBuilder_Definitions loaded = MyGuiBlueprintScreenBase.LoadPrefab(filename);

            foreach (MyObjectBuilder_ShipBlueprintDefinition blueprint in loaded.ShipBlueprints)
            {
                _Blueprint = blueprint;
                Convert();
                return;
            }
        }

        private void Convert()
        {
            _GridTerminalSystem = new GridTerminalSystem();
            if (IsLoaded)
            {
                foreach (MyObjectBuilder_CubeGrid grid in _Blueprint.CubeGrids)
                {
                    foreach (MyObjectBuilder_CubeBlock block in grid.CubeBlocks)
                    {
                        _GridTerminalSystem.AddBlock((IMyTerminalBlock)block);
                    }
                }
            }
        }
    }
}
