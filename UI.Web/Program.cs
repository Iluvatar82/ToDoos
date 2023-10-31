using Framework.Repositories;
using Framework.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDo.Data.Identity;
using ToDo.Data.ToDoData;
using UI.Web.Areas.Identity;
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
            builder.Services.AddTransient<IdentityRepository>();

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
    }
}