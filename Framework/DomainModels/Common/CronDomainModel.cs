namespace Framework.DomainModels.Common
{
    public class CronDomainModel
    {
        public string Definition { get; set; } = string.Empty;

        public static implicit operator string(CronDomainModel model) => model.Definition;
        public static implicit operator CronDomainModel(string cron) => new CronDomainModel() { Definition = cron };

        public override string ToString() => Definition;
    }
}
