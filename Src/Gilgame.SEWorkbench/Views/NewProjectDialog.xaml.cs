using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class NewProjectDialog : Window
    {
        public string ProjectName { get; set; }

        public string ProjectLocation { get; set; }

        public NewProjectDialog()
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

        private bool ValidName(string text)
        {
            if (text.IndexOfAny(Services.Strings.InvalidFilenameChars) > -1)
            {
                return false;
            }
            return true;
        }
    }
}
