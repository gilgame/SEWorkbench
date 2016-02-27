using System;
using System.IO;
using System.Linq;
using System.Windows;

using Gilgame.SEWorkbench.ViewModels;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace Gilgame.SEWorkbench.Views
{
    public partial class TestWindow : Window
    {
        private ProjectManagerViewModel _ProjectManager = new ProjectManagerViewModel(null);

        public TestWindow()
        {
            InitializeComponent();
            RegisterEvents();
            SetDataContext();
            LoadWindowLayout();
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

            //vFindReplace.DataContext = _ProjectManager.FindReplace;

            DockManager.DataContext = _ProjectManager.Editor;

            vOutput.DataContext = _ProjectManager.Output;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!_ProjectManager.HandleClosing())
            {
                e.Cancel = true;
            }
            else
            {
                SaveWindowLayout();
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
                ShowAnchorable("Output", AnchorableShowStrategy.Bottom);
            }
        }

        private void OutputView_ErrorMessageSelected(object sender, ErrorMessageEventArgs e)
        {
            bool found = _ProjectManager.FindError(e.Output);
            if (found)
            {
                foreach (PageViewModel page in _ProjectManager.Editor.Items)
                {
                    if (page.Filename == e.Output.Filename)
                    {
                        page.IsActive = true;
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDockLayout();
        }

        private void LoadWindowLayout()
        {
            Width = Configuration.MainWindow.Width;
            Height = Configuration.MainWindow.Height;
            Left = Configuration.MainWindow.Left;
            Top = Configuration.MainWindow.Top;

            WindowState = Configuration.MainWindow.WindowState;
        }

        private void LoadDockLayout()
        {
            string path = GetDockLayoutPath();
            if (File.Exists(path))
            {
                using (var dispatch = Dispatcher.DisableProcessing())
                {
                    XmlLayoutSerializer layoutSerializer = new XmlLayoutSerializer(DockManager);
                    layoutSerializer.LayoutSerializationCallback += (s, args) =>
                    {
                        args.Content = args.Content;
                    };

                    using (var reader = new StreamReader(path))
                    {
                        layoutSerializer.Deserialize(reader);
                    }
                }
            }
        }

        private string GetDockLayoutPath()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string workbench = Path.Combine(appdata, "SEWorkbench");
            string layout = Path.Combine(workbench, "layout.xml");

            if (!Directory.Exists(workbench))
            {
                Directory.CreateDirectory(workbench);
            }

            return layout;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            _ProjectManager.VerifyFiles();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState != WindowState.Minimized)
            {
                _ProjectManager.VerifyFiles();
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveDockLayout();
        }

        private void SaveWindowLayout()
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
        }

        private void SaveDockLayout()
        {
            string path = GetDockLayoutPath();
            XmlLayoutSerializer layoutSerializer = new XmlLayoutSerializer(DockManager);
            using (var writer = new StreamWriter(path))
            {
                layoutSerializer.Serialize(writer);
            }
        }

        private void ProjectExplorerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAnchorable("Project", AnchorableShowStrategy.Left);
        }

        private void BlueprintsViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAnchorable("Blueprints", AnchorableShowStrategy.Right);
        }

        private void ClassesViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAnchorable("Classes", AnchorableShowStrategy.Right);
        }

        private void OutputViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAnchorable("Output", AnchorableShowStrategy.Bottom);
        }

        private void ShowAnchorable(string id, AnchorableShowStrategy strategy)
        {
            var anchorable = DockManager.Layout.Descendents().OfType<LayoutAnchorable>().Single(a => a.ContentId == id);
            if (anchorable.IsHidden)
            {
                anchorable.Show();
            }
            else if (anchorable.IsVisible)
            {
                anchorable.IsActive = true;
            }
            else
            {
                anchorable.AddToLayout(DockManager, strategy | AnchorableShowStrategy.Most);
            }
        }

        private void FindReplaceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //if (pnFindReplace.IsHidden)
            //{
            //    pnFindReplace.Show();
            //    if (pnFindReplace.IsAutoHidden)
            //    {
            //        pnFindReplace.ToggleAutoHide();
            //    }
            //}
        }

        private void DevMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/gilgame/SEWorkbench");
        }

        private void BugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/gilgame/SEWorkbench/issues");
        }

        private void DocMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/gilgame/SEWorkbench/wiki");
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AboutView().ShowDialog();
        }

        private void PreferencesItem_Click(object sender, RoutedEventArgs e)
        {
            Config.EditorView dialog = new Config.EditorView(_ProjectManager.Config);
            dialog.Show();
        }
    }
}
