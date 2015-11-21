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

        public static void Import(string filename, out string name, out Interop.Grid grid)
        {
            name = string.Empty;
            grid = new Interop.Grid();

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

                    grid.Name = name;
                    foreach (MyObjectBuilder_CubeGrid cubegrid in blueprints.CubeGrids)
                    {
                        foreach (MyObjectBuilder_CubeBlock block in cubegrid.CubeBlocks)
                        {
                            if (block is MyObjectBuilder_TerminalBlock)
                            {
                                MyObjectBuilder_TerminalBlock terminalblock = (MyObjectBuilder_TerminalBlock)block;

                                string type = GetBlockType(block.TypeId.ToString());

                                // TODO Use MyTexts.GetString(MyStringId id) to get default blocks names from MyTexts.resx ?
                                string customname = String.IsNullOrEmpty(terminalblock.CustomName) ? type : terminalblock.CustomName;

                                grid.AddBlock(type, new TerminalBlock() { Name = customname });
                            }
                        }
                    }

                    // loaded.ShipBlueprints
                }

                

                //Save(@"C:\Users\Tim\Desktop\result\bp.sbc", loaded);
            }

            return;
        }

        private static void Save(string path, MyObjectBuilder_Base blueprint)
        {
            MyObjectBuilderSerializer.SerializeXML(path, false, blueprint);
        }


        private static string GetBlockType(string name)
        {
            return name.Replace("MyObjectBuilder_", "");
        }
    }
}
