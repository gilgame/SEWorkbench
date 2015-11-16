using System;
using System.Text;

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Gui;
using Sandbox.Game.Entities;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.Entities.Cube;
using System.Reflection;


namespace Gilgame.SEWorkbench.Interop
{
    /// <summary>
    /// Imports blueprint from file and converts to GridTerminalSystem
    /// </summary>
    public class Blueprint
    {
        public static void Import(string filename, out string name, out GridTerminalSystem gridterminalsystem)
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sepath = String.Format("{0}\\SpaceEngineers", appdata);

            // TODO load init, this should probably happen somewhere else
            VRage.FileSystem.MyFileSystem.Init(sepath, sepath);

            MyObjectBuilder_Definitions loaded = MyGuiBlueprintScreenBase.LoadPrefab(filename);

            name = string.Empty;
            gridterminalsystem = null;
            foreach (MyObjectBuilder_ShipBlueprintDefinition blueprints in loaded.ShipBlueprints)
            {
                name = blueprints.DisplayName;
                foreach (MyObjectBuilder_CubeGrid grid in blueprints.CubeGrids)
                {
                    try
                    {
                        gridterminalsystem = new GridTerminalSystem();

                        var cubegrid = MyEntities.CreateFromObjectBuilder(grid) as MyCubeGrid;
                        foreach (var block in cubegrid.GetBlocks())
                        {
                            if (block.FatBlock != null)
                            {
                                var functional = block.FatBlock as MyTerminalBlock;
                                if (functional != null)
                                {
                                    gridterminalsystem.AddBlock(functional);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException is ReflectionTypeLoadException)
                        {
                            ReflectionTypeLoadException lex = (ReflectionTypeLoadException)ex.InnerException;
                            foreach (var item in lex.LoaderExceptions)
                            {
                                System.Windows.MessageBox.Show(item.Message);
                            }
                        }
                    }

                    return; // TODO support for multiple grids per blueprint?
                }
            }
        }
    }
}
