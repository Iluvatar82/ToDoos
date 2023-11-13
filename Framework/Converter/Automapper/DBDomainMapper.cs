using AutoMapper;

namespace Framework.Converter.Automapper
{
    public class DBDomainMapper
    {
        protected IMapper Mapper { get; set; }
        public DBDomainMapper(IMapper mapper) => Mapper = mapper;


        public TDest Convert<TSource, TDest>(TSource source) => Mapper.Map<TDest>(source);
    }
}
