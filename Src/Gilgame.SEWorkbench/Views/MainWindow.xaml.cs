using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.CodeCompletion;

namespace Gilgame.SEWorkbench.Views
{
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

            ViewModels.EditorViewModel vm = new ViewModels.EditorViewModel();
            vm.Items.Add(new ViewModels.PageViewModel("Test1.csx") { IsSelected = true });
            vm.Items.Add(new ViewModels.PageViewModel("Test2.csx"));

            tcFileEditor.DataContext = vm;

            //_Completion = new ICSharpCode.CodeCompletion.CSharpCompletion(new Completion.ScriptProvider());
            //OpenFile2(@"NewFile.csx");
            //OpenFile(@"NewFile.csx");

            //Gilgame.SEWorkbench.Interop.InGameScript script = new Interop.InGameScript("void main(){int id = 1;}");
            //MessageBox.Show(script.LastError);
            //foreach (string error in script.CompileErrors)
            //{
            //    MessageBox.Show(error);
            //}
        }

        private void OpenFile(string fileName)
        {
            var editor = new CodeTextEditor();
            editor.FontFamily = new FontFamily("Consolas");
            editor.FontSize = 11;
            editor.Completion = _Completion;
            editor.OpenFile(fileName);
            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            editor.Margin = new Thickness(0, 6, 0, 6);

            ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };
            editor.Options = options;

            var tabItem = new TabItem();
            tabItem.Content = editor;
            tabItem.Header = "CloseDoors.csx"; // System.IO.Path.GetFileName(fileName);
            tcFileEditor.Items.Add(tabItem);
        }

        private void OpenFile2(string fileName)
        {
            var editor = new CodeTextEditor();
            editor.FontFamily = new FontFamily("Consolas");
            editor.FontSize = 11;
            editor.Completion = _Completion;
            editor.OpenFile(fileName);
            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            editor.Margin = new Thickness(0, 6, 0, 6);

            ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };
            editor.Options = options;

            var tabItem = new TabItem();
            tabItem.Content = editor;
            tabItem.Header = "DisableTurrets.csx"; // System.IO.Path.GetFileName(fileName);
            tcFileEditor.Items.Add(tabItem);
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            tvProjectExplorer.StartNewProject();
        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            tvProjectExplorer.OpenProject();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            tvProjectExplorer.SaveProject();
        }
    }
}
