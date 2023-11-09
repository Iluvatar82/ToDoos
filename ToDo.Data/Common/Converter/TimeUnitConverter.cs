using ToDo.Data.Common.Enums;

namespace ToDo.Data.Common.Converter
{
    public static class TimeUnitConverter
    {
        public static string Convert(this ScheduleTimeUnit unit)
        {
            switch (unit)
            {
                case ScheduleTimeUnit.Minute:
                    return "m";

                case ScheduleTimeUnit.Hour:
                    return "h";

                case ScheduleTimeUnit.Day:
                    return "d";

                case ScheduleTimeUnit.Week:
                    return "w";

                case ScheduleTimeUnit.Month:
                    return "M";

                case ScheduleTimeUnit.Year:
                    return "y";

                default:
                    return "d";
            }
        }

        public static ScheduleTimeUnit Convert(this string  unitString)
        {
            switch (unitString)
            {
                case "m":
                    return ScheduleTimeUnit.Minute;

                case "h":
                    return ScheduleTimeUnit.Hour;

                case "d":
                    return ScheduleTimeUnit.Day;

                case "w":
                    return ScheduleTimeUnit.Week;

                case "M":
                    return ScheduleTimeUnit.Month;

                case "y":
                    return ScheduleTimeUnit.Year;

                default:
                    return ScheduleTimeUnit.Day;
            }
        }
    }
}
