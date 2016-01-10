using System;
using System.Globalization;
using System.Windows.Data;

namespace Gilgame.SEWorkbench.Converters
{
    public class SelectedIsBlockConverter : IValueConverter
    {
        public double Length { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Models.GridItemType type = GetValue(value);
            switch (type)
            {
                case Models.GridItemType.Block:
                    return true;

                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        private Models.GridItemType GetValue(object o)
        {
            Models.GridItemType type = (Models.GridItemType)Enum.Parse(typeof(Models.GridItemType), o.ToString());
            return type;
        }
    }
}
