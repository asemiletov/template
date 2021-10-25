using System.Collections.Generic;
using System.Threading.Tasks;
using LementPro.Server.Common.ActionLog.Middleware.Attributes;
using LementPro.Server.Common.Sdk.Models;
using LementPro.Server.SvcTemplate.Common;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LementPro.Server.SvcTemplate.Api.Controllers.Api
{
    [ApiController, ApiExplorerSettings(GroupName = "Public")]
    [Route("api/[controller]")]

    public class BlockWorkController : Controller
    {
       
        private readonly IBlockWorkUserAdapter _userAdapter;

        public BlockWorkController(IBlockWorkUserAdapter userAdapter)
        {
            _userAdapter = userAdapter;
        }

        /// <summary>
        /// Create new BlockWork.
        /// </summary>
        /// <remarks>
        /// ### Remark
        /// 
        /// ### Changelog
        /// * 2020-01-14 Initial 
        /// 
        /// </remarks>
        /// <returns>Created BlockWork identifier</returns>
        /// <response code="200" cref="long">Created BlockWork identifier</response>
        /// <response code="400" cref="ValidationResponse">Array of validation errors</response>
        /// <response code="500" cref="ErrorResponse">Reason of internal server error</response>
        [HttpPost]
        [LogAction(ActionLogAction.BlockWorAdd)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Add(BlockWorkAddModel model) => Ok(await _userAdapter.Add(model));
        
        /// <summary>
        /// Get BlockWork list.
        /// </summary>
        /// <remarks>
        /// ### Remark
        /// 
        /// ### Changelog
        /// * 2020-01-14 Initial 
        /// 
        /// </remarks>
        /// <returns>Result with BlockWork enumerate</returns>
        /// <response code="200" cref="IEnumerable{T}">List of BlockWorks</response>
        /// <response code="400" cref="ValidationResponse">Array of validation errors</response>
        /// <response code="422" cref="ErrorResponse">Reason of server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BlockWorkModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> List() => Ok(await _userAdapter.List());

        /// <summary>
        /// Get BlockWork by identifier.
        /// </summary>
        /// <remarks>
        /// ### Remark
        /// 
        /// ### Changelog
        /// * 2020-01-14 Initial 
        /// 
        /// </remarks>
        /// <returns>Result with BlockWork enumerate</returns>
        /// <response code="200" cref="IEnumerable{T}">List of BlockWorks</response>
        /// <response code="400" cref="ValidationResponse">Array of validation errors</response>
        /// <response code="404" cref="ErrorResponse">Not found message</response>
        /// <response code="422" cref="ErrorResponse">Reason of server error</response>
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(BlockWorkModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Get([FromRoute]long id) => Ok(await _userAdapter.Get(id));


        /// <summary>
        /// Delete BlockWork by identifier.
        /// </summary>
        /// <remarks>
        /// ### Remark
        /// 
        /// ### Changelog
        /// * 2020-01-14 Initial 
        /// 
        /// </remarks>
        /// <returns>Empty result</returns>
        /// <response code="200">Empty result</response>
        /// <response code="400" cref="ValidationResponse">Array of validation errors</response>
        /// <response code="422" cref="ErrorResponse">Reason of server error</response>
        [HttpDelete, Route("{id}")]
        [ApiExplorerSettings(GroupName = "Maintenance")]
        [LogAction(ActionLogAction.BlockWorkDelete)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Delete([FromRoute]long id)
        {
            await _userAdapter.Delete(id);
            return new StatusCodeResult(204);
        }
    }
}
