using Hangfire;
using ServiceJobs;

namespace UI.Web.StartupConfiguration
{
    public static class ConfigureServices
    {
        public static void Register(IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate(nameof(DatabaseCleanupJob), () => app.ApplicationServices.GetService<DatabaseCleanupJob>()!.Execute(), "*/30 * * * *", new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local });
        }
    }
}
