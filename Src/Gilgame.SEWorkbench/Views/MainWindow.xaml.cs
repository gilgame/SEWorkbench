using System;
using System.Windows;

using Gilgame.SEWorkbench.ViewModels;

namespace Gilgame.SEWorkbench.Views
{
    public partial class MainWindow : Window
    {
        private ProjectManagerViewModel _ProjectManager = new ProjectManagerViewModel(null);

        public MainWindow()
        {
            InitializeComponent();
            RegisterEvents();
            SetDataContext();
        }

        private void RegisterEvents()
        {
            _ProjectManager.CloseViewRequested += ProjectManager_CloseViewRequested;
        }

        private void SetDataContext()
        {
            DataContext = _ProjectManager;

            tvBlueprint.DataContext = _ProjectManager.Blueprint;
            tcFileEditor.DataContext = _ProjectManager.Editor;
            tvProjectExplorer.DataContext = _ProjectManager.Project;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!_ProjectManager.HandleClosing())
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        private void ProjectManager_CloseViewRequested(object sender, EventArgs e)
        {
            Close();
        }
    }
}
