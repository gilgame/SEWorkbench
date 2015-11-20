using System;

using Gilgame.SEWorkbench.ViewModels;

namespace Gilgame.SEWorkbench.Views
{
    public delegate void BlueprintSelectedEventHandler(object sender, BlueprintSelectedEventArgs e);

    public class BlueprintSelectedEventArgs : EventArgs
    {
        public ProjectItemViewModel Item { get; set; }
    }
}
