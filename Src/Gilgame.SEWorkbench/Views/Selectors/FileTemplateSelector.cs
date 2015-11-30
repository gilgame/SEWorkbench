using System;
using System.Windows;
using System.Windows.Controls;

namespace Gilgame.SEWorkbench.Views.Selectors
{
    public class FileTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ViewModels.PageViewModel)
            {
                return FileTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
