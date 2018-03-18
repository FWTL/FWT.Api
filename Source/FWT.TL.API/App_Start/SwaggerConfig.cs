using Auth.FWT.API.SwaggerExtensions;
using FWT.TL.API;
using Swashbuckle.Application;
using System.Web.Http;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace FWT.TL.API
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
            .EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Telegram.Server");
                c.OperationFilter<DefaultValueOperationFilter>();
                c.OperationFilter<UpdateFileDownloadOperations>();
                c.UseFullTypeNameInSchemaIds();
            })
            .EnableSwaggerUi(c =>
            {
                c.CustomAsset("index", thisAssembly, "Telegram.Server.SwaggerExtensions.Index.html");
            });
        }
    }
}