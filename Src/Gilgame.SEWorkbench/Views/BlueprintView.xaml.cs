using System;
using System.Windows.Controls;

using Gilgame.SEWorkbench.ViewModels;

namespace Gilgame.SEWorkbench.Views
{
    public partial class BlueprintView : UserControl
    {
        public BlueprintView()
        {
            InitializeComponent();
        }

        public void SetDataContext(ProjectItemViewModel item)
        {
            if (item == null)
            {
                DataContext = new Object();
            }
            else
            {
                DataContext = item;
                if (!item.IsExpanded)
                {
                    if (item.Grid.Count > 0)
                    {
                        item.Grid[0].IsExpanded = true;
                    }
                }
            }
        }
    }
}
