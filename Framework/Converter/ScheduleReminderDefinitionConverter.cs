using AutoMapper;
using Framework.Converter.Automapper;
using System.Text.RegularExpressions;
using ToDo.Data.Common;

namespace Framework.Converter
{
    public partial class ScheduleReminderDefinitionConverter : DBDomainMapper,
        IValueConverter<string, ReminderDefinition>, IValueConverter<ReminderDefinition, string>,
        ITypeConverter<string, ReminderDefinition>, ITypeConverter<ReminderDefinition, string>
    {
        public ScheduleReminderDefinitionConverter(IMapper mapper) : base(mapper)
        {
        }


        public ReminderDefinition Convert(string reminderString, ResolutionContext context)
        {
            var result = new ReminderDefinition();
            var match = ReminderRegex().Match(reminderString);
            if (!match.Success)
                return result;

            if (decimal.TryParse(match.Groups["value"].Value, out var value) && match.Groups["unit"].Value.Length == 1)
            {
                result.Value = value;
                Mapper.Map(match.Groups["unit"].Value, result.Unit);
            }

            return result;
        }

        public string Convert(ReminderDefinition source, ResolutionContext context) => $"{source.Value:F2}{Mapper.Map<string>(source.Unit)}";

        public ReminderDefinition Convert(string source, ReminderDefinition destination, ResolutionContext context) => Convert(source, context);

        public string Convert(ReminderDefinition source, string destination, ResolutionContext context) => Convert(source, context);


        [GeneratedRegex("(?<value>\\d*(?:[\\.,]?\\d+)?)\\s*?(?<unit>[mhd])")]
        private static partial Regex ReminderRegex();
    }
}
