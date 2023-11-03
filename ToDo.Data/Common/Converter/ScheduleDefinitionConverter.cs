using System.Text.RegularExpressions;

namespace ToDo.Data.Common.Converter
{
    public static class ScheduleDefinitionConverter
    {
        public static ScheduleDefinition Convert(string scheduleDefinitionString)
        {
            var result = new ScheduleDefinition();
            if (string.IsNullOrWhiteSpace(scheduleDefinitionString))
                return result;

            switch (scheduleDefinitionString[0])
            {
                case 'd': //Deadline
                    if (DateTime.TryParse(new Regex("d\\s(?<time>.+)").Match(scheduleDefinitionString).Groups["time"].Value, out var deadline))
                        result.Deadline = deadline;
                    break;

                case 'w': //Days of week
                case 'i': //Intervall
                default:
                    break;
            }

            return result;
        }

        public static string Convert(ScheduleDefinition scheduleDefinition)
        {
            if (scheduleDefinition?.Deadline != null)
                return $"d {scheduleDefinition.Deadline}";

            return string.Empty;
        }
    }
}
