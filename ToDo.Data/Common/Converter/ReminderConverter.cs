using System.Text.RegularExpressions;

namespace ToDo.Data.Common.Converter
{
    public static partial class ReminderConverter
    {
        public static ReminderDefinition Convert(this string reminderString)
        {
            var result = new ReminderDefinition();
            var match = ReminderRegex().Match(reminderString);
            if (!match.Success)
                return result;

            if (decimal.TryParse(match.Groups["value"].Value, out var value) && match.Groups["unit"].Value.Length == 1)
            {
                result.Value = value;
                result.Unit = TimeUnitConverter.Convert(match.Groups["unit"].Value);
            }

            return result;
        }

        public static string Convert(this ReminderDefinition reminderDefinition) => reminderDefinition.ToString();


        [GeneratedRegex("(?<value>\\d*(?:[\\.,]?\\d+)?)\\s*?(?<unit>[mhd])")]
        private static partial Regex ReminderRegex();
    }
}
