using System.ComponentModel.DataAnnotations;

namespace Framework.DomainModels.Base
{
    public class SettingDomainModel : DomainModelBase
    {
        public Guid UserId { get; set; }

        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
