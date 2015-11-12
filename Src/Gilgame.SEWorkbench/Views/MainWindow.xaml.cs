using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gilgame.SEWorkbench.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICSharpCode.CodeCompletion.CSharpCompletion _Completion;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _Completion = new ICSharpCode.CodeCompletion.CSharpCompletion(new Completion.ScriptProvider());
            OpenFile(@"C:\Users\tfellows\Documents\GitHub\SEWorkbench\SampleFiles\SampleScript1.csx");
            //OpenFile(@"C:\Users\tfellows\Documents\GitHub\SEWorkbench\SampleFiles\Sample1.cs");
        }

        private void OpenFile(string fileName)
        {
            var editor = new CodeTextEditor();
            editor.FontFamily = new FontFamily("Consolas");
            editor.FontSize = 12;
            editor.Completion = _Completion;
            editor.OpenFile(fileName);
            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");

            var tabItem = new TabItem();
            tabItem.Content = editor;
            tabItem.Header = System.IO.Path.GetFileName(fileName);
            FileEditorTabControl.Items.Add(tabItem);

        }
    }
}
