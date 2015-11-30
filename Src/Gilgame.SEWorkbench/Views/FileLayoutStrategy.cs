using System;
using System.Linq;

using Xceed.Wpf.AvalonDock.Layout;

namespace Gilgame.SEWorkbench.Views
{
    public class FileLayoutStrategy : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorable, ILayoutContainer destination)
        {
            LayoutAnchorablePane pane = destination as LayoutAnchorablePane;
            if (destination != null && destination.FindParent<LayoutFloatingWindow>() != null)
            {
                return false;
            }

            LayoutAnchorablePane files = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "pnFiles");
            if (files != null)
            {
                files.Children.Add(anchorable);
                return true;
            }
            return false;
        }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorable)
        {
            return;
        }


        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument document, ILayoutContainer destination)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument document)
        {
            return;
        }
    }
}
