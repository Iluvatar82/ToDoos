using Framework.DomainModels.Common;
using Framework.DomainModels.Common.Enums;

namespace Framework.DomainModels.Base
{
    public class ScheduleDomainModel : DomainModelBase
    {
        public Guid ToDoItemId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public required ScheduleDefinition ScheduleDefinition { get; set; }

        public ScheduleType Type
        {
            get
            {
                if (ScheduleDefinition.Fixed == null && ScheduleDefinition.Interval == null && ScheduleDefinition.WeekDays == null)
                    return default;

                if (ScheduleDefinition.Fixed != null)
                    return ScheduleType.Fixed;

                if (ScheduleDefinition.Interval != null)
                    return ScheduleType.Interval;

                if (ScheduleDefinition.WeekDays != null)
                    return ScheduleType.WeekDays;

                return default;
            }
        }
    }
}
