using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views.Config
{
    public partial class EditorView : Window, INotifyPropertyChanged
    {
        private TextEditorPage _TextEditorPage = new TextEditorPage();

        private object _ConfigPage;
        public object ConfigPage
        {
            get
            {
                return _ConfigPage;
            }
            set
            {
                if (_ConfigPage != value)
                {
                    _ConfigPage = value;
                    OnPropertyChanged("ConfigPage");
                }
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

        public EditorView()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox)
            {
                ListBox list = (ListBox)sender;

                ListBoxItem item = (ListBoxItem)list.SelectedItem;
                switch (item.Content.ToString())
                {
                    case "Text Editor":
                        ConfigPage = _TextEditorPage;
                        break;
                }
            }
        }
    }
}
