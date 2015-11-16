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
            BuildTree();
        }

        private void BuildTree()
        {
            //_Project = new ViewModels.ProjectViewModel();
            _Project = ViewModels.ProjectViewModel.NewProject("C:/Users/Tim/Documents/SEWorkbench", "TestProject");

            //Models.ProjectItem root = GetProjectItems(_Project);

            //_Project.SetRootItem(root);

            //_Project.OpenProject(@"C:\Users\Tim\Documents\SEWorkbench\AllianceFleet\AllianceFleet.seproj");

            DataContext = _Project;
        }

        private Models.ProjectItem GetProjectItems(ViewModels.ProjectViewModel project)
        {
            return new Models.ProjectItem
            {
                Name = "AllianceFleet",
                Type = Models.ProjectItemType.Root,
                Path = "AllianceFleet",
                Project = project,
                Children =
                {
                    new Models.ProjectItem
                    {
                        Name = "Dreadnaught",
                        Type = Models.ProjectItemType.Folder,
                        Path = "AllianceFleet/Dreadnaught",
                        Project = project,
                        Children =
                        {
                            new Models.ProjectItem
                            {
                                Name = "CloseDoors",
                                Type = Models.ProjectItemType.File,
                                Path = "AllianceFleet/Dreadnaught/CloseDoors.csx",
                                Project = project
                            },
                            new Models.ProjectItem
                            {
                                Name = "DisableTurrets",
                                Type = Models.ProjectItemType.File,
                                Path = "AllianceFleet/Dreadnaught/DisableTurrets.csx",
                                Project = project
                            },
                        }
                    }
                }
            };
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
