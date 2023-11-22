using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Framework.Converter;
using Framework.Converter.Automapper;
using Framework.Repositories;
using Framework.Services;
using Framework.Services.Base;
using Hangfire;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.Identity;
using ToDo.Data.ToDoData;
using UI.Web.Areas.Identity;
using UI.Web.Areas.Identity.EmailSenderWrapper;
using UI.Web.Hangfire;
using UI.Web.Hubs;
using UI.Web.Services;

namespace UI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionName = IsDevelopment() && false ? "DBConnection_Local" : "DBConnection";
            var connectionString = builder.Configuration.GetConnectionString(connectionName) ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContextFactory<ToDoDBContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
  
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });

            builder.Services.AddServerSideBlazor();

            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            builder.Services.AddScoped<ToastNotificationService>();
            
            builder.Services.AddTransient<CategoryRepository>();
            builder.Services.AddTransient<ItemRepository>();
            builder.Services.AddTransient<ListRepository>();
            builder.Services.AddTransient<ScheduleRepository>();
            builder.Services.AddTransient<ScheduleReminderRepository>();
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<SettingRepository>();

            builder.Services.AddTransient<IdentityRepository>();

            builder.Services.AddTransient<ItemStyleService>();
            builder.Services.AddTransient<ItemDragDropService>();
            builder.Services.AddTransient<ItemHandlerService>();
            builder.Services.AddTransient<ReminderService>();

            builder.Services.AddSingleton<DBDomainMapper>();

            builder.Services.AddTransient<EmailService>();
            builder.Services.AddTransient<IEmailSender, EmailSenderWrapper>();

            AddEmailSenderSecrets(builder);

            builder.Services.AddAntiforgery(af => af.SuppressXFrameOptionsHeader = true);
            builder.Services.AddCors();

            ConfigureHangfireService(builder.Services, connectionString);
            ConfigureAutoMapper(builder.Services);

            builder.Services.AddTransient<ModelMapper>();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var url = builder.WebHost.GetSetting("applicationUrl");
            var hangfireOptions = new DashboardOptions()
            {
                Authorization = new[] { new DashboardAuthorizationFilter() },
                AppPath = "/todos/",
            };

            app.UseHangfireDashboard("/hangfire", hangfireOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();
            app.MapBlazorHub();
            app.MapHub<GroupListUpdateHub>(GroupListUpdateHub.HubUrl);
            app.MapFallbackToPage("/_Host");

            app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.Run();
        }

        private static void ConfigureHangfireService(IServiceCollection services, string connectionString)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString));

            services.AddHangfireServer();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<ToDoDBContext>(serviceProvider);
            }, typeof(ToDoDBContext).Assembly, typeof(MapperProfile).Assembly);
        }

        private static void AddEmailSenderSecrets(WebApplicationBuilder builder)
        {
            var keyVaultUrl = builder.Configuration.GetValue<string>("AzureKeyVault:AzureKeyVaultURL");
            var clientId = builder.Configuration.GetValue<string>("AzureKeyVault:AzureClientId");
            var clientSecret = builder.Configuration.GetValue<string>("AzureKeyVault:AzureClientSecret");
            var tenantId = builder.Configuration.GetValue<string>("AzureKeyVault:AzureClientTenantId");
            
            var client = new SecretClient(new Uri(keyVaultUrl!), new ClientSecretCredential(tenantId, clientId, clientSecret));

            builder.Services.Configure<AuthMessageSenderOptions>(options => {
                options.SendGridKey = client.GetSecret("SendGridKey").Value.Value;
                options.Email = new Email()
                {
                    Sender_Address = client.GetSecret("EmailSenderAddress").Value.Value,
                    Sender_Display_Name = client.GetSecret("EmailSenderDisplayName").Value.Value
                };
            });
        }

        private static bool IsDevelopment()
            => string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
    }
}