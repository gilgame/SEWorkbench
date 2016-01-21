using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace ICSharpCode.AvalonEdit.Highlighting
{
    public class WordHighlighter : DocumentColorizingTransformer
    {
        private TextEditor _TextEditor; DocumentLine currentDocumentLine;

        public WordHighlighter(TextEditor editor)
        {
            _TextEditor = editor;
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            //if (_TextEditor == null)
            //{
            //    return;
            //}

            //string word = _TextEditor.SelectedText;
            //if (!(new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9_]+$").IsMatch(word)))
            //{
            //    return;
            //}


            //int offset = line.Offset;
            //DocumentLine docline = _TextEditor.TextArea.Document.GetLineByOffset(offset);
            //string text = CurrentContext.Document.GetText(docline);
            //int start = 0;
            //int index = 0;
            //while ((index = text.IndexOf(word, start)) >= 0)
            //{
            //    try
            //    {
            //        base.ChangeLinePart(
            //            offset + index, // startOffset
            //            offset + index + word.Length, // endOffset
            //            (VisualLineElement element) =>
            //            {
            //                element.BackgroundBrush = new SolidColorBrush(Colors.Silver);
            //            });
            //    }
            //    catch (Exception ex)
            //    {
            //        // do stuff
            //    }
            //    start = index + word.Length; // search for next occurrence
            //}
        }
    }
}
