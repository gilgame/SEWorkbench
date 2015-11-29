using System;
using System.Globalization;
using System.Windows.Data;

namespace Gilgame.SEWorkbench.Converters
{
    public class ClassItemTypeImageConverter : IValueConverter
    {
        public double Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool expanded = false;
            if (parameter != null)
            {
                expanded = (bool)parameter;
            }

            Models.ClassItemType type = GetValue(value);
            switch (type)
            {
                case Models.ClassItemType.Namespace:
                    return "/Gilgame.SEWorkbench;component/Icons/namespace.gif";

                case Models.ClassItemType.Object:
                    return "/Gilgame.SEWorkbench;component/Icons/class.gif";

                case Models.ClassItemType.Field:
                    return "/Gilgame.SEWorkbench;component/Icons/field.gif";

                case Models.ClassItemType.Property:
                    return "/Gilgame.SEWorkbench;component/Icons/property.gif";

                case Models.ClassItemType.Method:
                    return "/Gilgame.SEWorkbench;component/Icons/method.gif";

                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        private Models.ClassItemType GetValue(object o)
        {
            Models.ClassItemType type = (Models.ClassItemType)Enum.Parse(typeof(Models.ClassItemType), o.ToString());
            return type;
        }
    }
}
