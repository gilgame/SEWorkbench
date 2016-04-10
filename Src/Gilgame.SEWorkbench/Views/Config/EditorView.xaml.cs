using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views.Config
{
    public partial class EditorView : Window, INotifyPropertyChanged
    {
        private ViewModels.Config.ConfigViewModel _Config;

        private TextEditorPage _TextEditorPage;
        private UpdaterPage _UpdaterPage;

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

        public EditorView(ViewModels.Config.ConfigViewModel config)
        {
            InitializeComponent();
            SetDataContext(config);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigPage = _TextEditorPage;
        }

        private void SetDataContext(ViewModels.Config.ConfigViewModel config)
        {
            _Config = config;

            _TextEditorPage = new TextEditorPage(_Config.TextEditor);
            _UpdaterPage = new UpdaterPage(_Config.Updater);

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

                    case "Updater":
                        ConfigPage = _UpdaterPage;
                        break;
                }
            }
        }
    }
}
