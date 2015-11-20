using Sandbox.Common.ObjectBuilders.Definitions;
using System;
using System.IO;
using VRage.ObjectBuilders;


namespace Gilgame.SEWorkbench.Interop
{
    /// <summary>
    /// Imports blueprint from file and converts to GridTerminalSystem
    /// </summary>
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

        public static void Import(string filename, out string name, out GridTerminalSystem gridterminalsystem)
        {
            name = string.Empty;
            gridterminalsystem = null;

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

                    gridterminalsystem = new GridTerminalSystem();
                }
            }
        }

        //public static void Import(string filename, out string name, out GridTerminalSystem gridterminalsystem)
        //{
        //    name = string.Empty;
        //    gridterminalsystem = null;

        //    MyObjectBuilder_CubeGrid dirty = new MyObjectBuilder_CubeGrid();

        //    MyObjectBuilder_Definitions loaded = MyGuiBlueprintScreenBase.LoadPrefab(filename);
        //    if (loaded == null)
        //    {
        //        string message = String.Format("Failed to load blueprint ({0}).", filename);
        //        System.Windows.MessageBox.Show(message);

        //        return;
        //    }

        //    foreach (MyObjectBuilder_ShipBlueprintDefinition blueprints in loaded.ShipBlueprints)
        //    {
        //        name = blueprints.Id.SubtypeId;
        //        foreach (MyObjectBuilder_CubeGrid grid in blueprints.CubeGrids)
        //        {
        //            try
        //            {
        //                gridterminalsystem = new GridTerminalSystem();

        //                //Sandbox.Game.Entities.MyEntities.IgnoreMemoryLimits = true;
        //                var cubegrid = Gilgame.SEWorkbench.Interop.Lib.MyEntities.CreateFromObjectBuilder(grid) as MyCubeGrid;
        //                foreach (var block in cubegrid.GetBlocks())
        //                {
        //                    if (block.FatBlock != null)
        //                    {
        //                        var functional = block.FatBlock as MyTerminalBlock;
        //                        if (functional != null)
        //                        {
        //                            gridterminalsystem.AddBlock(functional);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                if (ex.InnerException is ReflectionTypeLoadException)
        //                {
        //                    ReflectionTypeLoadException lex = (ReflectionTypeLoadException)ex.InnerException;
        //                    foreach (var item in lex.LoaderExceptions)
        //                    {
        //                        System.Windows.MessageBox.Show(item.Message);
        //                    }
        //                }
        //            }

        //            return; // TODO support for multiple grids per blueprint?
        //        }
        //    }
        //}
    }
}
