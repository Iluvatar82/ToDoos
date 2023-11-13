using AutoMapper;
using Framework.Converter.Automapper;
using ToDo.Data.Common.Enums;

namespace Framework.Converter
{
    public class TimeUnitConverter : DBDomainMapper,
        IValueConverter<string, ScheduleTimeUnit>, IValueConverter<ScheduleTimeUnit, string>,
        ITypeConverter<string, ScheduleTimeUnit>, ITypeConverter<ScheduleTimeUnit, string>
    {
        public TimeUnitConverter(IMapper mapper) : base(mapper)
        {
        }


        public ScheduleTimeUnit Convert(string unitString, ResolutionContext context)
        {
            var result = unitString switch
            {
                "m" => ScheduleTimeUnit.Minute,
                "h" => ScheduleTimeUnit.Hour,
                "d" => ScheduleTimeUnit.Day,
                "w" => ScheduleTimeUnit.Week,
                "M" => ScheduleTimeUnit.Month,
                "y" => ScheduleTimeUnit.Year,
                _ => ScheduleTimeUnit.Day,
            };

            return result;
        }

        public string Convert(ScheduleTimeUnit unit, ResolutionContext context)
        {
            var result = unit switch
            {
                ScheduleTimeUnit.Minute => "m",
                ScheduleTimeUnit.Hour => "h",
                ScheduleTimeUnit.Day => "d",
                ScheduleTimeUnit.Week => "w",
                ScheduleTimeUnit.Month => "M",
                ScheduleTimeUnit.Year => "y",
                _ => "d",
            };

            return result;
        }

        public ScheduleTimeUnit Convert(string source, ScheduleTimeUnit destination, ResolutionContext context) => Convert(source, context);
        
        public string Convert(ScheduleTimeUnit source, string destination, ResolutionContext context) => Convert(source, context);
    }
}
