using System;
using System.Windows;

using Gilgame.SEWorkbench.ViewModels;
using Gilgame.SEWorkbench.Services;

namespace Gilgame.SEWorkbench.Views
{
    public partial class MainWindow : Window
    {
        private ProjectManagerViewModel _ProjectManager = new ProjectManagerViewModel(null);

        private OutputView _OutputView = new OutputView() { Name = "OutputWindow" };

        public MainWindow()
        {
            InitializeComponent();
            RegisterEvents();
            SetDataContext();
        }

        private void RegisterEvents()
        {
            _ProjectManager.CloseViewRequested += ProjectManager_CloseViewRequested;
            _ProjectManager.ScriptRunning += ProjectManager_ScriptRunning;

            _OutputView.ErrorMessageSelected += OutputView_ErrorMessageSelected;
        }

        private void SetDataContext()
        {
            DataContext = _ProjectManager;

            tvBlueprint.DataContext = _ProjectManager.Blueprint;
            tcFileEditor.DataContext = _ProjectManager.Editor;
            tvProjectExplorer.DataContext = _ProjectManager.Project;
            tvClasses.DataContext = _ProjectManager.Classes;

            _OutputView.DataContext = _ProjectManager.Output;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!_ProjectManager.HandleClosing())
            {
                e.Cancel = true;
            }
            else
            {
                _OutputView.CloseWindow = true;
                _OutputView.Close();
            }

            base.OnClosing(e);
        }

        private void ProjectManager_CloseViewRequested(object sender, EventArgs e)
        {
            Close();
        }

        private void ProjectManager_ScriptRunning(object sender, EventArgs e)
        {
            if (_ProjectManager.Output.Items.Count > 0)
            {
                if (!_OutputView.IsLoaded)
                {
                    _OutputView.Show();
                }
                else
                {
                    _OutputView.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void OutputView_ErrorMessageSelected(object sender, ErrorMessageEventArgs e)
        {
            Focus();

            _ProjectManager.FindError(e.Output);
        }
    }
}
