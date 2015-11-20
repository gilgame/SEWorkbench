using System;
using System.Collections.Generic;
using System.IO;

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;


namespace Gilgame.SEWorkbench.Interop
{
    public class Blueprint
    {
        private static bool Initialized = false;

        /// <summary>
        /// Runs the SE init. Do this first
        /// </summary>
        public static void RunInit()
        {
            if (!Initialized)
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string sepath = String.Format("{0}\\SpaceEngineers", appdata);

                VRage.FileSystem.MyFileSystem.Init(sepath, sepath);

                Initialized = true;
            }
        }

        public static void Import(string filename, out string name, out Dictionary<string, List<TerminalBlock>> blocks)
        {
            name = string.Empty;
            blocks = new Dictionary<string, List<TerminalBlock>>();

            MyObjectBuilder_Definitions loaded = null;
            if (File.Exists(filename))
            {
                using (FileStream stream = File.Open(filename, FileMode.Open))
                {
                    MyObjectBuilderSerializer.DeserializeXML(stream, out loaded);
                }
            }

            if (loaded != null)
            {
                foreach (MyObjectBuilder_ShipBlueprintDefinition blueprints in loaded.ShipBlueprints)
                {
                    name = blueprints.Id.SubtypeId;

                    foreach (MyObjectBuilder_CubeGrid grid in blueprints.CubeGrids)
                    {
                        foreach (MyObjectBuilder_CubeBlock block in grid.CubeBlocks)
                        {
                            if (block is MyObjectBuilder_TerminalBlock)
                            {
                                MyObjectBuilder_TerminalBlock terminalblock = (MyObjectBuilder_TerminalBlock)block;

                                string type = GetBlockType(block.TypeId.ToString());
                                string customname = String.IsNullOrEmpty(terminalblock.CustomName) ? type : terminalblock.CustomName;

                                if (!blocks.ContainsKey(type))
                                {
                                    List<TerminalBlock> list = new List<TerminalBlock>();
                                    blocks.Add(type, list);
                                }

                                blocks[type].Add(new TerminalBlock() { Name = customname });
                            }
                        }
                    }

                    // loaded.ShipBlueprints
                }
            }

            return;
        }

        private static string GetBlockType(string name)
        {
            return name.Replace("MyObjectBuilder_", "");
        }
    }
}
