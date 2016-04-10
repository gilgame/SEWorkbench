using System;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views.Config
{
    public partial class UpdaterPage : UserControl
    {
        public UpdaterPage(ViewModels.Config.UpdaterViewModel updater)
        {
            InitializeComponent();
            SetDataContext(updater);
        }

        private void SetDataContext(ViewModels.Config.UpdaterViewModel updater)
        {
            DataContext = updater;
        }
    }
}
