using System.Drawing;

namespace Framework.Services
{
    public class UIColorService
    {
        public static string GetTextColor(string backgroundColor)
        {
            var convertedColor = Core.Converter.ColorConverter.GetColor(backgroundColor);
            if (convertedColor.GetBrightness() < 0.66)
                return Core.Converter.ColorConverter.GetString(Color.FromArgb(255, 225, 225, 225));

            return Core.Converter.ColorConverter.GetString(Color.FromArgb(255, 88, 88, 88));
        }

        public static string GetDoneTextColor(string backgroundColor) => Core.Converter.ColorConverter.GetString(Color.FromArgb(255, 20, 66, 20));
    }
}
