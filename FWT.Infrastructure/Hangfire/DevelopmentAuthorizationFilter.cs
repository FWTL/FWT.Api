using Hangfire.Dashboard;

namespace FWT.Infrastructure.Hangfire
{
    public class DevelopmentAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}