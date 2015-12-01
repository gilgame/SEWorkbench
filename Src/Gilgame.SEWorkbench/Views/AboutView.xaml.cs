using System;
using System.Windows;

namespace Gilgame.SEWorkbench.Views
{
    public partial class AboutView : Window
    {
        public string Title { get; set; }

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
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {

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
