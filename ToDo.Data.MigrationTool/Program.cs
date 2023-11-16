using Framework.Converter.Automapper;
using Framework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ToDo.Data.MigrationTool.ManualMigrations;
using ToDo.Data.ToDoData;

namespace ToDo.Data.MigrationTool
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var connectionName = IsDevelopment() ? "DBConnection_Local" : "DBConnection";
            var connectionString = builder.Configuration.GetConnectionString(connectionName) ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");

            builder.Services.AddDbContextFactory<ToDoDBContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddTransient<ItemRepository>();

            ///Define Workload-Class
            builder.Services.AddScoped<IMigrator, ItemDeadline_To_Schedule_Migrator>();
            
            builder.Services.AddHostedService<ConsoleHostedService>();

            ConfigureAutoMapper(builder.Services);

            var app = builder.Build();
            app.Run();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
        }

        private static bool IsDevelopment()
        {
            return string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
        }
    }

    internal sealed class ConsoleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IMigrator _migrator;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IMigrator migrator)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _migrator = migrator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation("Hello World!");

                        await _migrator.MigrateAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                    }
                    finally
                    {
                        _appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}