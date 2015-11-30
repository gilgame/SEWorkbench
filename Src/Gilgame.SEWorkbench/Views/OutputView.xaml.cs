using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

using Gilgame.SEWorkbench.ViewModels;

namespace Gilgame.SEWorkbench.Views
{
    public partial class OutputView : UserControl
    {
        private bool _CloseWindow = false;
        public bool CloseWindow
        {
            get
            {
                return _CloseWindow;
            }
            set
            {
                _CloseWindow = value;
            }
        }

        public event ErrorMessageEventHandler ErrorMessageSelected;
        private void RaiseErrorMessageSelected(OutputItemViewModel output)
        {
            if (output != null && ErrorMessageSelected != null)
            {
                ErrorMessageSelected(this, new ErrorMessageEventArgs(output));
            }
        }

        public OutputView()
        {
            InitializeComponent();
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(lvErrors.SelectedItem != null)
            {
                OutputItemViewModel item = lvErrors.SelectedItem as OutputItemViewModel;
                Clipboard.SetText(item.Message);
            }
        }

        private void ErrorsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(lvErrors.SelectedItem != null)
            {
                OutputItemViewModel item = lvErrors.SelectedItem as OutputItemViewModel;
                RaiseErrorMessageSelected(item);
            }
        }
    }
}
