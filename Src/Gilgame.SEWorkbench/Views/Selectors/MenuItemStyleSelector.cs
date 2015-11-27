using System;
using System.Windows;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Selectors
{
    public class MenuItemStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item == null)
            {
                return ((FrameworkElement)container).FindResource("mSeparatorStyle") as Style;
            }
            return base.SelectStyle(item, container);
        }
    }
}
