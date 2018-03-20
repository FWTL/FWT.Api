using FWT.TL.API;
using FWT.TL.Infrastructure.Telegram;
using System.Web.Http;

namespace Telegram.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new TelegramContractResolver();
        }
    }
}
