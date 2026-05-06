using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace VibyApp.UI.Converters
{
    public class IndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[0];
            var items = values[1] as IList;

            if (items == null || item == null)
                return "";

            return items.IndexOf(item) + 1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
