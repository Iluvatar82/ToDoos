namespace ToDo.Data.MigrationTool.ManualMigrations
{
    internal interface IMigrator
    {
        Task MigrateAsync();
    }
}
