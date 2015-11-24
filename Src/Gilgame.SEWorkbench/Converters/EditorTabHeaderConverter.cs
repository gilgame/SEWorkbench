using System;
using System.Globalization;
using System.Windows.Data;

namespace Gilgame.SEWorkbench.Converters
{
    public class EditorTabHeaderConverter : IMultiValueConverter
    {
        public double Length { get; set; }

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Length == 2)
            {
                if (!(value[0] is String) || !(value[1] is Boolean))
                {
                    return null;
                }

                string name = value[0].ToString();

                bool modified = (bool)value[1];
                if (modified)
                {
                    return name + "*";
                }
                else
                {
                    return name;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
