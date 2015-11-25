using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class ProjectExplorer : UserControl
    {
        public ProjectExplorer()
        {
            InitializeComponent();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ViewModels.ProjectViewModel)
            {
                ViewModels.ProjectViewModel project = (ViewModels.ProjectViewModel)DataContext;

                project.SearchCommand.Execute(null);
            }
        }

        private void ProjectExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ViewModels.ProjectViewModel)
            {
                ViewModels.ProjectViewModel project = (ViewModels.ProjectViewModel)DataContext;

                project.RaiseFileRequested();
            }
        }
    }
}
