using System;
using System.IO;

using Sandbox.Common.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Game;


namespace Gilgame.SEWorkbench.Interop
{
    public class Blueprint
    {
        public static void Import(string filename, out string name, out Interop.Grid grid)
        {
            name = string.Empty;
            grid = null;

            MyObjectBuilder_Definitions loaded = null;
            if (File.Exists(filename))
            {
                using (FileStream stream = File.Open(filename, FileMode.Open))
                {
                    MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(stream, out loaded);
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
            try
            {
                string backup = String.Format("{0}.bak", path);
                if (!File.Exists(backup))
                {
                    File.Copy(path, backup);
                }
                else
                {
                    FileInfo source = new FileInfo(path);
                    FileInfo dest = new FileInfo(path);

                    if (source.LastWriteTime > dest.LastWriteTime)
                    {
                        File.Copy(path, backup, true);
                    }
                }

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

                MyObjectBuilderSerializer.SerializeXML(path, false, definitions);
            }
            catch (Exception ex)
            {
                string message = String.Format(
                    "{0} ({1}){2}{2}{3}",
                    "There was an error saving the blueprint",
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace
                );

                System.Windows.MessageBox.Show(
                    message,
                    "SE Workbench",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }

            return definitions;
        }

        public static MyObjectBuilder_Definitions SaveBlockName(string path, MyObjectBuilder_Definitions definitions, long entityid, string name)
        {
            try
            {
                string backup = String.Format("{0}.bak", path);
                if (!File.Exists(backup))
                {
                    File.Copy(path, backup);
                }
                else
                {
                    FileInfo source = new FileInfo(path);
                    FileInfo dest = new FileInfo(path);

                    if (source.LastWriteTime > dest.LastWriteTime)
                    {
                        File.Copy(path, backup, true);
                    }
                }

                foreach (MyObjectBuilder_ShipBlueprintDefinition blueprints in definitions.ShipBlueprints)
                {
                    foreach (MyObjectBuilder_CubeGrid cubegrid in blueprints.CubeGrids)
                    {
                        foreach (MyObjectBuilder_CubeBlock block in cubegrid.CubeBlocks)
                        {
                            if (block is MyObjectBuilder_TerminalBlock)
                            {
                                if (block.EntityId == entityid)
                                {
                                    MyObjectBuilder_TerminalBlock term = (MyObjectBuilder_TerminalBlock)block;
                                    term.CustomName = name;
                                }
                            }
                        }
                    }
                }

                MyObjectBuilderSerializer.SerializeXML(path, false, definitions);
            }
            catch (Exception ex)
            {
                string message = String.Format(
                    "{0} ({1}){2}{2}{3}",
                    "There was an error saving the blueprint",
                    ex.Message,
                    Environment.NewLine,
                    ex.StackTrace
                );

                System.Windows.MessageBox.Show(
                    message,
                    "SE Workbench",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }

            return definitions;
        }

        private static string GetBlockType(string name)
        {
            return name.Replace("MyObjectBuilder_", "");
        }
    }
}
