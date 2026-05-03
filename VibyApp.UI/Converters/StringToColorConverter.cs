using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VibyApp.UI.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush DefaultBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8")); // Gris
        private static readonly SolidColorBrush ErrorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F87171"));   // Rouge

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? errorMessage = value as string;
            string? fieldKeyword = parameter as string;

            // Protection contre les valeurs nulles
            if (string.IsNullOrEmpty(errorMessage) || string.IsNullOrEmpty(fieldKeyword))
            {
                return DefaultBrush;
            }

            string msg = errorMessage.ToLower();
            string key = fieldKeyword.ToLower();

            if (key == "nom" && msg.Contains("utilisateur"))
            {
                return DefaultBrush;
            }

            // Logique normale de détection : si le message contient le mot-clé
            if (msg.Contains(key))
            {
                return ErrorBrush;
            }

            return DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}