using FluentValidation;
using FWT.TL.API.Models;
using FWT.TL.Core.Extensions;
using FWT.TL.Core.Services.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace FWT.TL.API.Filters
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        private ILogger _logger;

        public ApiExceptionAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            if (filterContext.Exception is ValidationException)
            {
                var exception = filterContext.Exception as ValidationException;
                filterContext.Response = filterContext.Request.CreateResponse((HttpStatusCode)400, new ValidationResultModel(exception));
                return;
            }

            var exceptionId = Guid.NewGuid();
            filterContext.Response = filterContext.Request.CreateResponse((HttpStatusCode)500, exceptionId);
            var sb = new StringBuilder();
            sb.AppendLine("ErrorId: " + exceptionId);
            sb.AppendLine(filterContext.Request.RequestUri.AbsoluteUri);
            sb.AppendLine();
            filterContext.ActionContext.Request.GetRouteData().Values.ForEach(parameter => sb.Append($"{parameter.Key} = {parameter.Value}").AppendLine());
            sb.AppendLine();
            sb.Append(filterContext.Exception);

            _logger.Error(sb.ToString());
        }
    }
}
