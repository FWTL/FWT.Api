using FWT.TL.API.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;
using Telegram.Server.App_Start;


[assembly: OwinStartup("api", typeof(FWT.TL.API.Bootstrapper.Startup))]
namespace FWT.TL.API.Bootstrapper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = IocConfig.RegisterDependencies();
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(System.Web.Http.GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(System.Web.Http.GlobalConfiguration.Configuration.Filters);

            MapperConfig.Configure();
            SwaggerConfig.Register();
            ConfigureAuth(app);

            app.Use((context, next) =>
            {
                context.Response.Headers.Remove("Server");
                return next.Invoke();
            });

            app.UseStageMarker(PipelineStage.PostAcquireState);
        }
    }
}
