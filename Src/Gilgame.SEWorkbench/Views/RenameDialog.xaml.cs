using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class RenameDialog : Window, INotifyPropertyChanged
    {
        private string _ItemName = String.Empty;
        public string ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                _ItemName = value;
                OnPropertyChanged("ItemName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public RenameDialog()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.SelectAll();
        }

        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!ValidChar(e.Text))
            {
                e.Handled = true;
            }
        }

        private void NameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidChar(e.Key.ToString()))
            {
                e.Handled = true;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PerformAdd();
        }

        private void PerformAdd()
        {
            if (String.IsNullOrEmpty(ItemName))
            {
                Services.MessageBox.ShowMessage("Name cannot be empty!");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            PerformCancel();
        }

        private void PerformCancel()
        {
            DialogResult = false;
            Close();
        }

        private bool ValidChar(string text)
        {
            Regex regex = new Regex(Services.Regex.BlockNameChar);
            if (!regex.IsMatch(text))
            {
                return false;
            }
            return true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                PerformCancel();
            }
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitButton.Focus();
                PerformAdd();
            }
        }
    }
}
