using Framework.Converter.Automapper;
using Framework.Repositories;
using Framework.Services;
using Framework.Services.Base;
using Hangfire;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.Identity;
using ToDo.Data.ToDoData;
using UI.Web.Areas.Identity;
using UI.Web.Areas.Identity.EmailSenderWrapper;
using UI.Web.Hangfire;
using UI.Web.Hubs;

namespace UI.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DBConnection") ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContextFactory<ToDoDBContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
  
            
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();


            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            builder.Services.AddScoped<ToastNotificationService>();
            
            builder.Services.AddTransient<CategoryRepository>();
            builder.Services.AddTransient<ItemRepository>();
            builder.Services.AddTransient<ListRepository>();
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<SettingRepository>();

            builder.Services.AddTransient<IdentityRepository>();

            builder.Services.AddSingleton<ReminderService>();
            builder.Services.AddSingleton<EmailService>();

            builder.Services.AddSingleton<DBDomainMapper>();

            builder.Services.AddTransient<IEmailSender, EmailSenderWrapper>();
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

            ConfigureHangfireService(builder.Services, connectionString);
            ConfigureAutoMapper(builder.Services);

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
            services.AddAutoMapper(typeof(MapperProfile).Assembly);
        }
    }
}