using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gilgame.SEWorkbench.Converters
{
    public class FileSelectedConverter : IValueConverter
    {
        public double Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.ProjectItemType type = GetValue(value);
            switch (type)
            {
                case Models.ProjectItemType.File:
                    return true;

                default:
                    return false;
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
