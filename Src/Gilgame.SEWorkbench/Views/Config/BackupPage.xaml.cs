using System;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views.Config
{
    public partial class BackupPage : UserControl
    {
        public BackupPage(ViewModels.Config.BackupViewModel backup)
        {
            InitializeComponent();
            SetDataContext(backup);
        }

        private void SetDataContext(ViewModels.Config.BackupViewModel backup)
        {
            DataContext = backup;
        }
    }
}
