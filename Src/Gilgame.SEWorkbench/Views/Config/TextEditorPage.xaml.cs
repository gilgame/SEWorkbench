using System;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views.Config
{
    public partial class TextEditorPage : UserControl
    {
        public TextEditorPage(ViewModels.Config.TextEditorViewModel editor)
        {
            InitializeComponent();
            SetDataContext(editor);
        }

        private void SetDataContext(ViewModels.Config.TextEditorViewModel editor)
        {
            DataContext = editor;
        }
    }
}
