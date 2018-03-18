using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace FWT.TL.API.SwaggerExtensions
{
    public class UpdateFileDownloadOperations : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var customAttributes = apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerFileResponseAttribute>();

            if (customAttributes.Any())
            {
                operation.produces = new[] { "application/octet-stream" };
                operation.responses["200"].schema = new Schema { type = "file" };
            }
        }
    }
}
