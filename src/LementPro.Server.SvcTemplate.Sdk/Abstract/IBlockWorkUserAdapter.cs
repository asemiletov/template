using System.Collections.Generic;
using System.Threading.Tasks;
using LementPro.Server.Common.Exception;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;

namespace LementPro.Server.SvcTemplate.Sdk.Abstract
{
    public interface IBlockWorkUserAdapter 
    {
        /// <summary>
        /// Sample delete method
        ///
        /// <remarks>
        /// Delete BlockWork item
        /// </remarks>
        /// 
        /// </summary>
        /// <exception cref="ServiceException{T}"></exception>
        Task Delete(long id);

        /// <summary>
        /// Sample method
        ///
        /// <remarks>
        /// Creates new BlockWork item
        /// </remarks>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Created model identifier</returns>
        /// <exception cref="ServiceException{T}"></exception>
        Task<long> Add(BlockWorkAddModel model);

        /// <summary>
        /// Sample method
        ///
        /// <remarks>
        /// List BlockWork items
        /// </remarks>
        /// 
        /// </summary>
        /// <returns>List of BlockWork items</returns>
        /// <exception cref="ServiceException{T}"></exception>
        Task<IEnumerable<BlockWorkModelSimple>> List();

        /// <summary>
        /// Sample method
        ///
        /// <remarks>
        /// Get BlockWork item by identifier
        /// </remarks>
        /// 
        /// </summary>
        /// <returns>Get BlockWork item</returns>
        /// <exception cref="ServiceException{T}"></exception>
        Task<BlockWorkModel> Get(long id);
    }
}