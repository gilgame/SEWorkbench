using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.CodeCompletion;

using Gilgame.SEWorkbench.ViewModels;

namespace Gilgame.SEWorkbench.Views
{
    public partial class MainWindow : Window
    {
        private ICSharpCode.CodeCompletion.CSharpCompletion _Completion;

        private Regex _LineColRegex = new Regex(@"\(([0-9]+),([0-9]+)\)");

        private ProjectManagerViewModel _ProjectManager = new ProjectManagerViewModel(null);

        public MainWindow()
        {
            InitializeComponent();
            SetDataContext();

            tvProjectExplorer.BlueprintSelected += ProjectExplorer_BlueprintSelected;
        }

        private void SetDataContext()
        {
            DataContext = _ProjectManager;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _Completion = new ICSharpCode.CodeCompletion.CSharpCompletion(new Completion.ScriptProvider());

            //ViewModels.EditorViewModel vm = new ViewModels.EditorViewModel();
            //vm.Items.Add(new ViewModels.PageViewModel("Test1.csx") { IsSelected = true });
            //vm.Items.Add(new ViewModels.PageViewModel("Test2.csx"));

            //tcFileEditor.DataContext = vm;
            //tvProjectExplorer.SetEditor(vm);

            //Gilgame.SEWorkbench.Interop.InGameScript script = new Interop.InGameScript("void main(){int id = 1;}");
            //MessageBox.Show(script.LastError);
            //foreach (string error in script.CompileErrors)
            //{
            //    MessageBox.Show(error);
            //}
        }

        private void ProjectExplorer_BlueprintSelected(object sender, BlueprintSelectedEventArgs e)
        {
            tvBlueprint.SetDataContext(e.Item);
        }

        private void OpenFile(string filename)
        {
            var editor = new CodeTextEditor();
            editor.FontFamily = new FontFamily("Consolas");
            editor.FontSize = 11;
            editor.Completion = _Completion;
            editor.OpenFile(filename);
            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            editor.Margin = new Thickness(0, 6, 0, 6);

            ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };
            editor.Options = options;

            var tabitem = new TabItem();
            tabitem.Content = editor;
            tabitem.Header = Path.GetFileNameWithoutExtension(filename);
            tcFileEditor.Items.Add(tabitem);

            SelectLastTab();
        }

        private void SelectLastTab()
        {
            if (tcFileEditor.Items.Count > 0)
            {
                tcFileEditor.SelectedIndex = tcFileEditor.Items.Count - 1;
            }
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            tvProjectExplorer.NewProject();
        }

        private void OpenProjectButton_Click(object sender, RoutedEventArgs e)
        {
            tvProjectExplorer.OpenProject();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            tvProjectExplorer.SaveProject();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO move logic to viewmodel
            TabItem tab = (TabItem)tcFileEditor.SelectedItem;
            if (tab != null)
            {
                if (tab.Content is CodeTextEditor)
                {
                    CodeTextEditor editor = (CodeTextEditor)tab.Content;

                    string code = editor.TextArea.Document.Text;

                    Interop.InGameScript script = new Interop.InGameScript(code);
                    foreach (string error in script.CompileErrors)
                    {
                        string message = error;

                        Match match = _LineColRegex.Match(error);
                        if (match.Groups.Count > 1)
                        {
                            int line = Convert.ToInt32(match.Groups[1].Value)-9;
                            int col = Convert.ToInt32(match.Groups[2].Value);


                            string linecol = String.Format("Line ({0}), Col ({1}):{2}{2}", line, col, Environment.NewLine);

                            message = linecol + message;
                        }

                        MessageBox.Show(message);
                    }
                }
            }
        }

        private void ProjectExplorer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProjectItemViewModel item = tvProjectExplorer.SelectedFile;
            if (item != null)
            {
                OpenFile(item.Path);
            }
        }

        private void FileEditor_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
