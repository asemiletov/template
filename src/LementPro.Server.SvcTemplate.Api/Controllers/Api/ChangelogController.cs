using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace LementPro.Server.SvcTemplate.Api.Controllers.Api
{
    /// <summary>
    /// Download changelog
    /// </summary>
    [ApiController, ApiExplorerSettings(GroupName = "Maintenance")]
    [Route("api/[controller]")]
    public class ChangelogController : ControllerBase
    {
        /// <summary>
        /// Download changelog.md
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "changelog.md");
            return PhysicalFile(path, "application/octet-stream", "changelog.md");
        }
    }
}
