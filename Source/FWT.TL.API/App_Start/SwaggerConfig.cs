using FWT.TL.API.SwaggerExtensions;
using Swashbuckle.Application;
using System.Web.Http;

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
                c.SingleApiVersion("v1", "FWT.TL.API");
                c.OperationFilter<DefaultValueOperationFilter>();
                c.OperationFilter<UpdateFileDownloadOperations>();
                c.UseFullTypeNameInSchemaIds();
            })
            .EnableSwaggerUi(c =>
            {
                c.CustomAsset("index", thisAssembly, "FWT.TL.API.SwaggerExtensions.Index.html");
            });
        }
    }
}