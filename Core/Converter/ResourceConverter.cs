using Core.Validation;
using System.Globalization;

namespace Core.Converter
{
    public static class ResourceConverter
    {
        public static bool GetBool(this string resource)
        {
            bool.TryParse(resource, out bool result);
            return result;
        }

        public static int GetInt(this string resource)
        {
            int.TryParse(resource, out int result);
            return result;
        }

        public static float GetFloat(this string resource)
        {
            float.TryParse(resource, new CultureInfo("en-US"), out float result);
            return result;
        }

        public static double GetDouble(this string resource)
        {
            double.TryParse(resource, out double result);
            return result;
        }

        public static decimal GetDecimal(this string resource)
        {
            decimal.TryParse(resource, out decimal result);
            return result;
        }

        public static (int A, int R, int G, int B) GetColorValues(this string resource)
        {
            resource.Satisfies(s => s.Count(c => c == ',') == 3);
            resource.Satisfies(s => s.Replace(",", string.Empty).Replace(" ", string.Empty).All(c => char.IsDigit(c)));

            var values = resource.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            return (values[0], values[1], values[2], values[3]);
        }
    }
}
