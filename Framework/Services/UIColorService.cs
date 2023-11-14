using Framework.Converter;
using Framework.Services.Base;
using System.Drawing;

namespace Framework.Services
{
    public class UIColorService
    {
        private static float? brightnessThreshold {  get; set; }
        private static (int A, int R, int G, int B) darkColor { get; set; }
        private static (int A, int R, int G, int B) darkBackgroundColor { get; set; }
        private static (int A, int R, int G, int B) lightColor { get; set; }


        public static string GetTextColor(string backgroundColor)
        {
            brightnessThreshold ??= ServiceResources.BrightnessThreshold.GetFloat();
            darkColor = darkColor != default ? darkColor : ServiceResources.DarkTextColor.GetColorValues();
            lightColor = lightColor != default ? lightColor : ServiceResources.LightTextColor.GetColorValues();

            var convertedColor = Converter.ColorConverter.GetColor(backgroundColor);
            if (convertedColor.GetBrightness() < brightnessThreshold)
                return Converter.ColorConverter.GetString(Color.FromArgb(lightColor.A, lightColor.R, lightColor.G, lightColor.B));

            return Converter.ColorConverter.GetString(Color.FromArgb(darkColor.A, darkColor.R, darkColor.G, darkColor.B));
        }

        public static string GetSimpleBackgroundColor(string? color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return Converter.ColorConverter.GetString(Color.FromArgb(lightColor.A, lightColor.R, lightColor.G, lightColor.B));

            brightnessThreshold ??= ServiceResources.BrightnessThreshold.GetFloat();
            darkBackgroundColor = darkBackgroundColor != default ? darkBackgroundColor : ServiceResources.DarkBackgroundTextColor.GetColorValues();

            var convertedColor = Converter.ColorConverter.GetColor(color);
            if (convertedColor.GetBrightness() < brightnessThreshold)
                return Converter.ColorConverter.GetString(Color.FromArgb(darkBackgroundColor.A, darkBackgroundColor.R, darkBackgroundColor.G, darkBackgroundColor.B));

            return Converter.ColorConverter.GetString(Color.FromArgb(lightColor.A, lightColor.R, lightColor.G, lightColor.B));
        }

        public static string GetDoneTextColor(string backgroundColor) => Converter.ColorConverter.GetString(Color.FromArgb(255, 20, 66, 20));
    }
}
