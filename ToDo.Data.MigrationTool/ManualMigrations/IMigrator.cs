using AutoMapper;

namespace ToDo.Data.MigrationTool.ManualMigrations
{
    internal interface IMigrator
    {
        IMapper Mapper { get; }
        Task MigrateAsync();
    }
}
