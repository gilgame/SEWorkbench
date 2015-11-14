using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gilgame.SEWorkbench.Converters
{
    public class ItemTypeImageConverter : IValueConverter
    {
        public double Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool expanded = false;
            if (parameter != null)
            {
                expanded = (bool)parameter;
            }

            Models.ProjectItemType type = GetValue(value);
            switch (type)
            {
                case Models.ProjectItemType.Root:
                    return "/Gilgame.SEWorkbench;component/Icons/Book.png";

                case Models.ProjectItemType.Folder:
                    if (expanded)
                    {
                        return "/Gilgame.SEWorkbench;component/Icons/Folder.png";
                    }
                    else
                    {
                        return "/Gilgame.SEWorkbench;component/Icons/ClosedFolder.png";
                    }

                case Models.ProjectItemType.File:
                    return "/Gilgame.SEWorkbench;component/Icons/Page.png";

                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        private Models.ProjectItemType GetValue(object o)
        {
            Models.ProjectItemType type = (Models.ProjectItemType)Enum.Parse(typeof(Models.ProjectItemType), o.ToString());
            return type;
        }
    }
}
