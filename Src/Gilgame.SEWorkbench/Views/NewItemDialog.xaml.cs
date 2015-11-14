using System;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class NewItemDialog : Window
    {
        public string ItemName { get; set; }

        public NewItemDialog()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!ValidName(e.Text))
            {
                e.Handled = true;
            }
        }

        private void NameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidName(e.Key.ToString()))
            {
                e.Handled = true;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidName(string name)
        {
            // TODO valid name logic
            return true;
        }
    }
}
