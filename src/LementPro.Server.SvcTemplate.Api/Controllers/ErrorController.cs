using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LementPro.Server.SvcTemplate.Api.Controllers
{
    /// <inheritdoc />
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Public ctor
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ErrorController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ErrorController>();
        }

        /// <summary>
        /// Default action on error
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/error/{code}")]
        public IActionResult Index(int? code)
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;

            if (error != null)
                _logger.LogError(error, "Error.");

            HttpContext.Response.StatusCode = code.HasValue && code == 404 ? 404 : 500;

            return View(code == 404 ? "~/Views/Error/NotFound.cshtml" : "~/Views/Error/Index.cshtml", error);
        }
    }
}