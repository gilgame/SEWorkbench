using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class BlueprintView : UserControl
    {
        public BlueprintView()
        {
            InitializeComponent();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ViewModels.BlueprintViewModel)
            {
                ViewModels.BlueprintViewModel blueprint = (ViewModels.BlueprintViewModel)DataContext;

                blueprint.SearchCommand.Execute(null);
            }
        }
    }
}
