using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LementPro.Server.SvcTemplate.Api.Controllers
{
    /// <inheritdoc />
    public class HomeController : Controller
    {
        /// <summary>
        /// Default controller
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View($"~/Views/Home/Index.cshtml");
        }
    }
}