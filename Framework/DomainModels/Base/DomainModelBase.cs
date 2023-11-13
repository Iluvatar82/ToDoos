using AutoMapper.Configuration.Annotations;
using System.Dynamic;

namespace Framework.DomainModels.Base
{
    public abstract class DomainModelBase
    {
        public Guid Id { get; set; }

        [Ignore]
        private dynamic AddedValues { get; set; } = new ExpandoObject();

        public void Set<T>(string name, T value) => (AddedValues as ExpandoObject).TryAdd(name, value);
        public T Get<T>(string name) => (T)((AddedValues as IDictionary<string, object>)[name]);

    }
}
