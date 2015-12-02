using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views
{
    public partial class AboutView : Window
    {
        public string AppTitle { get; set; }

        public string Version { get; set; }

        public string Copyright { get; set; }

        public AboutView()
        {
            InitializeComponent();
            SetDataContext();
            LoadAssemblyDetails();
        }

        private void SetDataContext()
        {
            DataContext = this;
        }

        private void LoadAssemblyDetails()
        {
            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            object[] titleattrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (titleattrs.Length > 0)
            {
                AppTitle = (titleattrs[0] as AssemblyTitleAttribute).Title;
            }

            object[] copyattrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (copyattrs.Length > 0)
            {
                Copyright = (copyattrs[0] as AssemblyCopyrightAttribute).Copyright;
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            object o = lvDocuments.SelectedItem;
            if (o is ListViewItem)
            {
                ListViewItem item = (ListViewItem)o;

                string path = Directory.GetCurrentDirectory();

                string content = item.Content.ToString();
                switch(content)
                {
                    case "Readme File":
                        OpenDocument(Path.Combine(path, "readme.txt"));
                        break;

                    case "SE Workbench License":
                        OpenDocument(Path.Combine(path, "license-seworkbench.txt"));
                        break;

                    case "SharpDevelop License":
                        OpenDocument(Path.Combine(path, "license-sharpdevelop.txt"));
                        break;

                    case "Xceed Extended WPF License":
                        OpenDocument(Path.Combine(path, "license-avalondock.txt"));
                        break;

                    case "Icon Archive License":
                        OpenDocument(Path.Combine(path, "license-iconarchive.txt"));
                        break;
                }
            }
        }

        private void OpenDocument(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                Services.MessageBox.ShowError("One or more files may be missing or your installation may be corrupt", ex);
            }
        }

        private void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                Close();
            }
        }
    }
}
