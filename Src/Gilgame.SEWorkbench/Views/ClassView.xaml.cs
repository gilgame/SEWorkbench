using System;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views
{
    public partial class ClassView : UserControl
    {
        public ClassView()
        {
            InitializeComponent();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ViewModels.ClassViewModel)
            {
                ViewModels.ClassViewModel classes = (ViewModels.ClassViewModel)DataContext;

                classes.SearchCommand.Execute(null);
            }
        }
    }
}
