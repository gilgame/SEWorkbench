using System;
using System.IO;
using System.Windows;

using Gilgame.SEWorkbench.ViewModels;
using Gilgame.SEWorkbench.Services;

using Xceed.Wpf.AvalonDock.Layout.Serialization;
using Xceed.Wpf.AvalonDock.Layout;

namespace Gilgame.SEWorkbench.Views
{
    public partial class TestWindow : Window
    {
        private const string DockConfig = "dock.config";

        private ProjectManagerViewModel _ProjectManager = new ProjectManagerViewModel(null);

        public TestWindow()
        {
            InitializeComponent();
            RegisterEvents();
            SetDataContext();
        }

        private void RegisterEvents()
        {
            _ProjectManager.CloseViewRequested += ProjectManager_CloseViewRequested;
            _ProjectManager.ScriptRunning += ProjectManager_ScriptRunning;

            vOutput.ErrorMessageSelected += OutputView_ErrorMessageSelected;
        }

        private void SetDataContext()
        {
            DataContext = _ProjectManager;

            tvBlueprint.DataContext = _ProjectManager.Blueprint;
            tvProjectExplorer.DataContext = _ProjectManager.Project;
            tvClasses.DataContext = _ProjectManager.Classes;
            DockManager.DataContext = _ProjectManager.Editor;

            vOutput.DataContext = _ProjectManager.Output;
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

        private void ProjectManager_ScriptRunning(object sender, EventArgs e)
        {
            if (_ProjectManager.Output.Items.Count > 0)
            {
                pnOutput.IsActive = true;
                pnOutput.Show();
            }
        }

        private void OutputView_ErrorMessageSelected(object sender, ErrorMessageEventArgs e)
        {
            _ProjectManager.FindError(e.Output);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Width = Configuration.MainWindow.Width;
            Height = Configuration.MainWindow.Height;
            Left = Configuration.MainWindow.Left;
            Top = Configuration.MainWindow.Top;

            WindowState = Configuration.MainWindow.WindowState;
            

            //string path = Path.Combine(Directory.GetCurrentDirectory(), DockConfig);
            //if (File.Exists(path))
            //{
            //    XmlLayoutSerializer serializer = new XmlLayoutSerializer(DockManager);
            //    serializer.LayoutSerializationCallback += (s, args) =>
            //    {
            //        args.Content = args.Content;
            //    };
            //    serializer.Deserialize(path);
            //}
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                Configuration.MainWindow.Width = Width;
                Configuration.MainWindow.Height = Height;
                Configuration.MainWindow.Left = Left;
                Configuration.MainWindow.Top = Top;

                Configuration.MainWindow.WindowState = WindowState;
            }
            if (WindowState == WindowState.Maximized)
            {
                Configuration.MainWindow.WindowState = WindowState;
            }
            

            //string path = Path.Combine(Directory.GetCurrentDirectory(), DockConfig);

            //XmlLayoutSerializer serializer = new XmlLayoutSerializer(DockManager);
            //serializer.Serialize(path);
        }

        private void ProjectExplorerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(pnProjectExplorer.IsHidden)
            {
                pnProjectExplorer.Show();
                if (pnProjectExplorer.IsAutoHidden)
                {
                    pnProjectExplorer.ToggleAutoHide();
                }
            }
        }

        private void BlueprintsViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (pnBlueprints.IsHidden)
            {
                pnBlueprints.Show();
                if (pnBlueprints.IsAutoHidden)
                {
                    pnBlueprints.ToggleAutoHide();
                }
            }
        }

        private void ClassesViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (pnClasses.IsHidden)
            {
                pnClasses.Show();
                if (pnClasses.IsAutoHidden)
                {
                    pnClasses.ToggleAutoHide();
                }
            }
        }

        private void OutputViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (pnOutput.IsHidden)
            {
                pnOutput.Show();
                if (pnOutput.IsAutoHidden)
                {
                    pnOutput.ToggleAutoHide();
                }
            }
        }
    }
}
