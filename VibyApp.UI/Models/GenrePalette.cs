using System.Windows.Media;

namespace VibyApp.UI.Models
{
    public static class GenrePalette
    {
        private static readonly (string Start, string End)[] Gradients =
        {
            ("#E8115B", "#BC0E4A"),
            ("#1E3264", "#16254A"),
            ("#F59E0B", "#D97706"),
            ("#6366F1", "#4338CA"),
            ("#BA5D07", "#8E4705"),
            ("#10B981", "#059669"),
            ("#EC4899", "#BE185D"),
            ("#14B8A6", "#0F766E"),
            ("#8B5CF6", "#6D28D9"),
            ("#EF4444", "#B91C1C"),
            ("#3B82F6", "#1D4ED8"),
            ("#84CC16", "#4D7C0F"),
        };

        public static LinearGradientBrush CreateBrush(int index)
        {
            var (start, end) = Gradients[index % Gradients.Length];
            var brush = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString(start)!,
                (Color)ColorConverter.ConvertFromString(end)!,
                45);
            brush.Freeze();
            return brush;
        }
    }
}
