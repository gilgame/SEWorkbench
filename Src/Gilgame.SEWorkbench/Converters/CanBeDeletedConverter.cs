using System;
using System.Globalization;
using System.Windows.Data;

namespace Gilgame.SEWorkbench.Converters
{
    public class CanBeDeletedConverter : IValueConverter
    {
        public double Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.ProjectItemType type = GetValue(value);
            switch (type)
            {
                case Models.ProjectItemType.Root:
                    return false;

                case Models.ProjectItemType.None:
                    return false;

                default:
                    return true;
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
