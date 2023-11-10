using Hangfire.Dashboard;

namespace UI.Web.Hangfire
{
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole("Administrator");
        }
    }
}
