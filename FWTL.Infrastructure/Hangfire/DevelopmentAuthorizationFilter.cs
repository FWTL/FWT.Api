using Hangfire.Dashboard;

namespace FWTL.Infrastructure.Hangfire
{
    public class DevelopmentAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
