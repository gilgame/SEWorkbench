using System;
using System.Windows;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views.Selectors
{
    public class FileStyleSelector : StyleSelector
    {
        public Style FileStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ViewModels.PageViewModel)
            {
                return FileStyle;
            }
            return base.SelectStyle(item, container);
        }
    }
}
