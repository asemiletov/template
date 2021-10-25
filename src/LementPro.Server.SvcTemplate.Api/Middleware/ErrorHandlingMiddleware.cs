using System.Threading.Tasks;
using LementPro.Server.Common.Exception;
using LementPro.Server.Common.Sdk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LementPro.Server.SvcTemplate.Api.Middleware
{
    /// <summary>
    /// Middleware class for catching exceptions.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Public ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
        }

        /// <summary>
        /// Catch exceptions
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ServiceException<ErrorResponse> ex)
            {
                _logger.LogDebug($"Warning '{ex.Content.Error}': {ex.Content.Message}");

                context.Response.StatusCode = ex.HttpStatusCode;
                await HandleExceptionAsync(context.Response, ex.Content);
            }
            catch (ServiceException ex)
            {
                _logger.LogWarning(ex, $"Warning {context.Request.Method} {context.Request.Path}");

                context.Response.StatusCode = ex.HttpStatusCode;//. StatusCodes.Status422UnprocessableEntity;
                await HandleExceptionAsync(context.Response, ex.Content);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error {context.Request.Method} {context.Request.Path}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await HandleExceptionAsync(context.Response, new ErrorResponse(500, "Internal", ex.Message));
            }
        }

        private static Task HandleExceptionAsync(HttpResponse response, object model)
        {
            response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(model);
            return response.WriteAsync(result);
        }
    }
}