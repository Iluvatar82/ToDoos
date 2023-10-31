using Core.Converter;
using Framework.Services.Base;
using System.Drawing;

namespace Framework.Services
{
    public class UIColorService
    {
        private static float? brightnessThreshold {  get; set; }
        private static (int A, int R, int G, int B) darkColor { get; set; }
        private static (int A, int R, int G, int B) lightColor { get; set; }


        public static string GetTextColor(string backgroundColor)
        {
            brightnessThreshold ??= ServiceResources.BrightnessThreshold.GetFloat();
            darkColor = darkColor != default ? darkColor : ServiceResources.DarkTextColor.GetColorValues();
            lightColor = lightColor != default ? lightColor : ServiceResources.LightTextColor.GetColorValues();

            var convertedColor = Core.Converter.ColorConverter.GetColor(backgroundColor);
            if (convertedColor.GetBrightness() < brightnessThreshold)
                return Core.Converter.ColorConverter.GetString(Color.FromArgb(lightColor.A, lightColor.R, lightColor.G, lightColor.B));

            return Core.Converter.ColorConverter.GetString(Color.FromArgb(darkColor.A, darkColor.R, darkColor.G, darkColor.B));
        }

        public static string GetDoneTextColor(string backgroundColor) => Core.Converter.ColorConverter.GetString(Color.FromArgb(255, 20, 66, 20));
    }
}
