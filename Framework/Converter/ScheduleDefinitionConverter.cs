using AutoMapper;
using Core.Validation;
using Framework.Converter.Automapper;
using Framework.DomainModels.Common;
using System.Text.RegularExpressions;

namespace Framework.Converter
{
    public partial class ScheduleDefinitionConverter : DBDomainMapper, 
        IValueConverter<string, ScheduleDefinition>, IValueConverter<ScheduleDefinition, string>,
        ITypeConverter<string, ScheduleDefinition>, ITypeConverter<ScheduleDefinition, string>
    {
        public ScheduleDefinitionConverter(IMapper mapper) : base(mapper)
        {
        }


        public ScheduleDefinition Convert(string scheduleDefinitionString, ResolutionContext? context)
        {
            var result = new ScheduleDefinition();
            if (string.IsNullOrWhiteSpace(scheduleDefinitionString))
                return result;

            switch (scheduleDefinitionString[0])
            {
                case 'd': //Fixed
                    var matchDeadline = FixedRegex().Match(scheduleDefinitionString);
                    if (matchDeadline.Success && DateTime.TryParse(matchDeadline.Groups["time"].Value, out var deadline))
                        result.Fixed = deadline;
                    break;

                case 'w': //Days of week
                    var matchWeekdays = WeekdayRegex().Match(scheduleDefinitionString);
                    if (matchWeekdays.Success)
                    {
                        var weekdays = matchWeekdays.Groups["weekdays"].Value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        weekdays.Satisfies(w => w.Length == 7);
                        weekdays.Satisfies(w => w.All(c => c[0] == '1' || c[0] == '0'));

                        var weekdayActive = weekdays.Select(c => c == "1").ToList();
                        var time = new TimeOnly();
                        if (matchWeekdays.Groups.ContainsKey("time") && matchWeekdays.Groups["time"].Length > 0)
                            time = TimeOnly.Parse(matchWeekdays.Groups["time"].Value);

                        result.WeekDays = new ScheduleWeekdays() { Days = weekdayActive, Time = time };
                    }

                    break;

                case 'i': //Intervall
                    var matchInterval = IntervalRegex().Match(scheduleDefinitionString);
                    if (matchInterval.Success && decimal.TryParse(matchInterval.Groups["interval"].Value, out var interval) && matchInterval.Groups["unit"].Value.Length == 1)
                    {
                        result.Interval = new ScheduleInterval();
                        result.Interval.Interval = interval;
                        result.Interval.Unit = Mapper.Map(matchInterval.Groups["unit"].Value, result.Interval.Unit);
                    }

                    break;

                default:
                    break;
            }

            return result;
        }

        public string Convert(ScheduleDefinition scheduleDefinition, ResolutionContext? context)
        {
            if (scheduleDefinition?.Fixed != null)
                return $"d {scheduleDefinition.Fixed}";

            if (scheduleDefinition?.WeekDays != null)
                return $"w {scheduleDefinition.WeekDays}";

            if (scheduleDefinition?.Interval != null)
                return $"i {scheduleDefinition.Interval}";

            return string.Empty;
        }

        public ScheduleDefinition Convert(string source, ScheduleDefinition destination, ResolutionContext? context) => Convert(source, context);
        public string Convert(ScheduleDefinition source, string destination, ResolutionContext? context) => Convert(source, context);

        [GeneratedRegex("d\\s(?<time>.+)")]
        private static partial Regex FixedRegex();

        [GeneratedRegex("w\\s(?<weekdays>(?:[01],\\s*?){6}\\s*?[01])\\s?(?<time>.+)*")]
        private static partial Regex WeekdayRegex();

        [GeneratedRegex("i\\s(?<interval>\\d*[\\.,]?\\d*)\\s(?<unit>[hdwmy])")]
        private static partial Regex IntervalRegex();
    }
}
