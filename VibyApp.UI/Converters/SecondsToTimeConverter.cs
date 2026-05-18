using System.Globalization;
using System.Windows.Data;

namespace VibyApp.UI.Converters
{
    public class SecondsToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double seconds)
            {
                var time = TimeSpan.FromSeconds(Math.Max(0, seconds));
                return time.TotalHours >= 1
                    ? time.ToString(@"h\:mm\:ss")
                    : time.ToString(@"m\:ss");
            }

            return "0:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
