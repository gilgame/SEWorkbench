using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Gilgame.SEWorkbench.Views
{
    public partial class AddReferenceView : Window, INotifyPropertyChanged
    {
        private string _Filename = String.Empty;
        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                _Filename = value;
                OnPropertyChanged("Filename");
            }
        }

        private string _Initial = String.Empty;
        public string Initial
        {
            get
            {
                return _Initial;
            }
            set
            {
                _Initial = value;
                OnPropertyChanged("Initial");
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

        public AddReferenceView()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                PerformCancel();
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".csx",
                Filter = "Script File (.csx)|*.csx",
                Multiselect = false,
                InitialDirectory = _Initial
            };

            Nullable<bool> result = dialog.ShowDialog();
            if (result != null && result.Value == true)
            {
                Filename = dialog.FileName;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            PerformCancel();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PerformAdd();
        }

        private void PerformAdd()
        {
            if (String.IsNullOrEmpty(Filename))
            {
                Services.MessageBox.ShowMessage("Please choose a file!");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void PerformCancel()
        {
            DialogResult = false;
            Close();
        }
    }
}
