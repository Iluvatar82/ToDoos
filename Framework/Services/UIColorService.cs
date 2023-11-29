using Framework.Converter;
using Framework.Services.Base;
using System.Drawing;

namespace Framework.Services
{
    public class UIColorService
    {
        private static float? BrightnessThreshold {  get; set; }
        private static (int A, int R, int G, int B) DarkColor { get; set; }
        private static (int A, int R, int G, int B) DarkBackgroundColor { get; set; }
        private static (int A, int R, int G, int B) LightColor { get; set; }


        public static string GetTextColor(string backgroundColor)
        {
            BrightnessThreshold ??= ServiceResources.BrightnessThreshold.GetFloat();
            DarkColor = DarkColor != default ? DarkColor : ServiceResources.DarkTextColor.GetColorValues();
            LightColor = LightColor != default ? LightColor : ServiceResources.LightTextColor.GetColorValues();

            var convertedColor = Converter.ColorConverter.GetColor(backgroundColor);
            if (convertedColor.GetBrightness() < BrightnessThreshold)
                return Converter.ColorConverter.GetString(Color.FromArgb(LightColor.A, LightColor.R, LightColor.G, LightColor.B));

            return Converter.ColorConverter.GetString(Color.FromArgb(DarkColor.A, DarkColor.R, DarkColor.G, DarkColor.B));
        }

        public static string GetSimpleBackgroundColor(string? color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return Converter.ColorConverter.GetString(Color.FromArgb(LightColor.A, LightColor.R, LightColor.G, LightColor.B));

            BrightnessThreshold ??= ServiceResources.BrightnessThreshold.GetFloat();
            DarkBackgroundColor = DarkBackgroundColor != default ? DarkBackgroundColor : ServiceResources.DarkBackgroundTextColor.GetColorValues();

            var convertedColor = Converter.ColorConverter.GetColor(color);
            if (convertedColor.GetBrightness() < BrightnessThreshold)
                return Converter.ColorConverter.GetString(Color.FromArgb(DarkBackgroundColor.A, DarkBackgroundColor.R, DarkBackgroundColor.G, DarkBackgroundColor.B));

            return Converter.ColorConverter.GetString(Color.FromArgb(LightColor.A, LightColor.R, LightColor.G, LightColor.B));
        }

        public static string GetDoneTextColor(string backgroundColor) => Converter.ColorConverter.GetString(Color.FromArgb(255, 20, 66, 20));
    }
}
