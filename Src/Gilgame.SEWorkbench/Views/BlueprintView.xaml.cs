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
            DataContext = item;
        }
    }
}
