using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class NewProjectDialog : Window, INotifyPropertyChanged
    {
        private string _ProjectName = String.Empty;
        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }
            set
            {
                _ProjectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        private string _ProjectLocation = String.Empty;
        public string ProjectLocation
        {
            get
            {
                return _ProjectLocation;
            }
            set
            {
                _ProjectLocation = value;
                OnPropertyChanged("ProjectLocation");
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

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog view = new OpenFolderDialog();

            Nullable<bool> result = view.ShowDialog();
            if (result != null && result == true)
            {
                ProjectLocation = view.FolderName;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PerformAdd();
        }

        private void PerformAdd()
        {
            if (String.IsNullOrEmpty(ProjectName))
            {
                Services.MessageBox.ShowMessage("Project name cannot be empty!");
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

        private bool ValidName(string text)
        {
            if (text.IndexOfAny(Services.Strings.InvalidFilenameChars) > -1)
            {
                return false;
            }
            return true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    PerformAdd();
            //}
            if (e.Key == Key.Escape)
            {
                PerformCancel();
            }
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddButton.Focus();
                PerformAdd();
            }
        }
    }
}
