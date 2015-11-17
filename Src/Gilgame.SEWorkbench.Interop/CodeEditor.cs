using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Interop
{
    /// <summary>
    /// Wrapper class for AvalonEdit (CodeCompletion) text editor
    /// </summary>
    public class CodeEditorWrapper : UserControl
    {
        private ICSharpCode.CodeCompletion.CodeTextEditor _Editor;
        public ICSharpCode.CodeCompletion.CodeTextEditor Editor
        {
            get
            {
                return _Editor;
            }
        }

        #region CompletionProperty
        public static readonly DependencyProperty CompletionProperty =
            DependencyProperty.Register(
                "Completion",
                typeof(ICSharpCode.CodeCompletion.CSharpCompletion),
                typeof(CodeEditorWrapper),
                new PropertyMetadata(default(ICSharpCode.CodeCompletion.CSharpCompletion), OnPropertyChanged)
            );

        public ICSharpCode.CodeCompletion.CSharpCompletion Completion
        {
            get
            {
                return (ICSharpCode.CodeCompletion.CSharpCompletion)GetValue(CompletionProperty);
            }
            set
            {
                SetValue(CompletionProperty, value);
            }
        }
        #endregion

        #region FilenameProperty
        public static readonly DependencyProperty FilenameProperty =
            DependencyProperty.Register(
                "Filename",
                typeof(string),
                typeof(CodeEditorWrapper),
                new PropertyMetadata(default(string), OnPropertyChanged)
            );

        public string Filename
        {
            get
            {
                return (string)GetValue(FilenameProperty);
            }
            set
            {
                SetValue(FilenameProperty, value);
            }
        }
        #endregion

        #region SyntaxHighlightingProperty
        public static readonly DependencyProperty SyntaxHighlightingProperty =
            DependencyProperty.Register(
                "SyntaxHighlighting",
                typeof(ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition),
                typeof(CodeEditorWrapper),
                new PropertyMetadata(default(ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition), OnPropertyChanged)
            );

        public ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition SyntaxHighlighting
        {
            get
            {
                return (ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition)GetValue(SyntaxHighlightingProperty);
            }
            set
            {
                SetValue(SyntaxHighlightingProperty, value);
            }
        }
        #endregion

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CodeEditorWrapper source = d as CodeEditorWrapper;
            string name = e.Property.Name;

            switch (name)
            {
                case "Completion":
                    SetCompletion(source, (ICSharpCode.CodeCompletion.CSharpCompletion)e.NewValue);
                    break;

                case "Filename":
                    SetFilename(source, (string)e.NewValue);
                    break;

                case "SyntaxHighlighting":
                    SetSyntaxHighlighting(source, (ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition)e.NewValue);
                    break;
            }
        }

        public CodeEditorWrapper()
        {
            ICSharpCode.CodeCompletion.CodeTextEditor editor = new ICSharpCode.CodeCompletion.CodeTextEditor();

            editor.Margin = new System.Windows.Thickness(0);
            ICSharpCode.AvalonEdit.TextEditorOptions options = new ICSharpCode.AvalonEdit.TextEditorOptions()
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
            };
            editor.Options = options;

            AddChild(editor);

            _Editor = editor;
        }

        private static void SetCompletion(CodeEditorWrapper wrapper,  ICSharpCode.CodeCompletion.CSharpCompletion completion)
        {
            wrapper.Editor.Completion = completion;
        }

        private static void SetFilename(CodeEditorWrapper wrapper, string filename)
        {
            wrapper.Editor.FileName = filename;
            wrapper.Editor.OpenFile(filename);
        }

        private static void SetSyntaxHighlighting(CodeEditorWrapper wrapper, ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition highlighting)
        {
            wrapper.Editor.SyntaxHighlighting = highlighting;
        }
    }
}
