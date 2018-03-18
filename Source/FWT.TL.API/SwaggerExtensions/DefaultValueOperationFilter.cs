﻿using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace FWT.TL.API.SwaggerExtensions
{
    public class DefaultValueOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                return;
            }

            var actionParams = apiDescription.ActionDescriptor.GetParameters();
            var customAttributes = apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerDefaultValueAttribute>();

            foreach (var param in operation.parameters)
            {
                var actionParam = actionParams.FirstOrDefault(p => p.ParameterName == param.name);
                if (actionParam != null)
                {
                    if (actionParam.ParameterType == typeof(bool))
                    {
                        param.@default = false;
                    }

                    var customAttribute = customAttributes.FirstOrDefault(p => p.ParameterName == param.name);
                    if (customAttribute != null)
                    {
                        param.@default = customAttribute.Value;
                    }
                }
            }
        }
    }
}
