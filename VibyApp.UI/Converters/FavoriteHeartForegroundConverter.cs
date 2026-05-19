using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VibyApp.UI.Converters
{
    public class FavoriteHeartForegroundConverter : IValueConverter
    {
        private static readonly SolidColorBrush Red = new(Color.FromRgb(239, 68, 68));
        private static readonly SolidColorBrush Muted = new(Color.FromRgb(100, 116, 139));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true ? Red : Muted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
