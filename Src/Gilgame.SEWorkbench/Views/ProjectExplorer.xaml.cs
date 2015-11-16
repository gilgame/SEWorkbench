using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gilgame.SEWorkbench.Views
{
    public partial class ProjectExplorer : UserControl
    {
        private ViewModels.ProjectViewModel _Project;
        
        public ProjectExplorer()
        {
            InitializeComponent();
        }

        public void StartNewProject()
        {
            _Project = ViewModels.ProjectViewModel.NewProject();

            if (_Project != null)
            {
                _Project.SaveProject();

                DataContext = _Project;
            }
        }

        public void OpenProject()
        {
            _Project = new ViewModels.ProjectViewModel();
            _Project.OpenProject();

            if (_Project.First != null)
            {
                DataContext = _Project;
            }
        }

        public void SaveProject()
        {
            if (_Project != null)
            {
                _Project.SaveProject();
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
                _Project.SearchCommand.Execute(null);
            //}
        }

        private void ProjectTreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TODO select node on right click
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _Project.SaveProject();
        }
    }
}
