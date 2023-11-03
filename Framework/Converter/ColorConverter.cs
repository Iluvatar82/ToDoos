using Core.Validation;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Framework.Converter
{
    public static partial class ColorConverter
    {
        public static Color GetColor(string colorString)
        {
            colorString.Satisfies(c => c.Length == 7);
            colorString.Satisfies(c => c[0] == '#');
            colorString.Satisfies(c => ColorRegex().IsMatch(c));

            return Color.FromArgb(255,
                int.Parse(colorString[1..3], System.Globalization.NumberStyles.HexNumber),
                int.Parse(colorString[3..5], System.Globalization.NumberStyles.HexNumber),
                int.Parse(colorString[5..7], System.Globalization.NumberStyles.HexNumber));
        }

        public static string GetString(Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}";


        [GeneratedRegex("#[a-fA-F0-9]{6}")]
        private static partial Regex ColorRegex();
    }
}
