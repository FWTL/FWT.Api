using FWT.TL.API.Filters;
using FWT.TL.Infrastructure.Logging;
using System.Web.Http.Filters;

namespace FWT.TL.API.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new ApiExceptionAttribute(NLogLogger.Instance));
        }
    }
}
