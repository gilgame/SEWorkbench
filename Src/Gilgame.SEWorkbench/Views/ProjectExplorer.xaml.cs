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
            Models.ProjectItem root = GetProjectItems();

            _Project = new ViewModels.ProjectViewModel(root);

            DataContext = _Project;
        }

        private Models.ProjectItem GetProjectItems()
        {
            return new Models.ProjectItem
            {
                Name = "SS-BigDaddy",
                Type = Models.ProjectItemType.Folder,
                Children =
                {
                    new Models.ProjectItem { Name = "CloseDoors", Type = Models.ProjectItemType.File },
                    new Models.ProjectItem { Name = "DisableTurrets", Type = Models.ProjectItemType.File },
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
    }
}
