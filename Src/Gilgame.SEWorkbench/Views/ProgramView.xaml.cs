using System;
using System.Windows;

namespace Gilgame.SEWorkbench.Views
{
    public partial class ProgramView : Window
    {
        private string _Title = "SE Workbench";
        public string TitleText
        {
            get
            {
                return String.Format("{0} - {1} ({2})", _Title, Blueprint, Block);
            }
        }

        private string _Program = String.Empty;
        public string Program
        {
            get
            {
                return _Program;
            }
            set
            {
                _Program = value;
            }
        }

        private string _Blueprint = String.Empty;
        public string Blueprint
        {
            get
            {
                return _Blueprint;
            }
            set
            {
                _Blueprint = value;
            }
        }

        private string _Block = String.Empty;
        public string Block
        {
            get
            {
                return _Block;
            }
            set
            {
                _Block = value;
            }
        }

        public ProgramView()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
