using Core.Validation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converter
{
    public static class ColorConverter
    {
        public static Color GetColor(string colorString)
        {
            colorString.Satisfies(c => c.Length == 7);
            colorString.Satisfies(c => c[0] == '#');

            return Color.FromArgb(255,
                int.Parse(colorString[1..3], System.Globalization.NumberStyles.HexNumber),
                int.Parse(colorString[3..5], System.Globalization.NumberStyles.HexNumber),
                int.Parse(colorString[5..7], System.Globalization.NumberStyles.HexNumber));
        }

        public static string GetString(Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}
