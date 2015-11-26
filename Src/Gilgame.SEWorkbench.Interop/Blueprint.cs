using System;
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
            grid = null;

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

                    grid = new Grid(loaded)
                    {
                        Name = name,
                        Path = filename,
                    };

                    foreach (MyObjectBuilder_CubeGrid cubegrid in blueprints.CubeGrids)
                    {
                        foreach (MyObjectBuilder_CubeBlock block in cubegrid.CubeBlocks)
                        {
                            if (block is MyObjectBuilder_TerminalBlock)
                            {
                                MyObjectBuilder_TerminalBlock terminalblock = (MyObjectBuilder_TerminalBlock)block;

                                long entityid = terminalblock.EntityId;
                                string type = GetBlockType(terminalblock.TypeId.ToString());

                                // TODO Use MyTexts.GetString(MyStringId id) to get default blocks names from MyTexts.resx (for localization)
                                string customname = String.IsNullOrEmpty(terminalblock.CustomName) ? type : terminalblock.CustomName;

                                if (block is MyObjectBuilder_MyProgrammableBlock)
                                {
                                    MyObjectBuilder_MyProgrammableBlock prog = (MyObjectBuilder_MyProgrammableBlock)block;
                                    grid.AddBlock(type, new TerminalBlock() { Name = customname, EntityID = entityid, IsProgram = true, Program = prog.Program });
                                }
                                else
                                {
                                    grid.AddBlock(type, new TerminalBlock() { Name = customname, EntityID = entityid, IsProgram = false });
                                }
                            }
                        }
                    }
                }
            }
        }

        public static MyObjectBuilder_Definitions SaveProgram(string path, MyObjectBuilder_Definitions definitions, long entityid, string program)
        {
            foreach (MyObjectBuilder_ShipBlueprintDefinition blueprints in definitions.ShipBlueprints)
            {
                foreach (MyObjectBuilder_CubeGrid cubegrid in blueprints.CubeGrids)
                {
                    foreach (MyObjectBuilder_CubeBlock block in cubegrid.CubeBlocks)
                    {
                        if (block is MyObjectBuilder_MyProgrammableBlock)
                        {
                            if (block.EntityId == entityid)
                            {
                                MyObjectBuilder_MyProgrammableBlock prog = (MyObjectBuilder_MyProgrammableBlock)block;
                                prog.Program = program;
                            }
                        }
                    }
                }
            }
            Save(path, definitions);

            return definitions;
        }

        private static void Save(string path, MyObjectBuilder_Definitions definitions)
        {
            MyObjectBuilderSerializer.SerializeXML(path, false, definitions);
        }


        private static string GetBlockType(string name)
        {
            return name.Replace("MyObjectBuilder_", "");
        }
    }
}
