using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LementPro.Server.Common.Authentication.Extensions;
using LementPro.Server.Common.Context.Abstract;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;
using LementPro.Server.SvcTemplate.Service.Abstract;

namespace LementPro.Server.SvcTemplate.Api.Adapters.User
{
    public class BlockWorkUserAdapter : IBlockWorkUserAdapter
    {
        private readonly IBlockWorkService _service;
        private readonly long _companyId;

        /// <summary>
        /// .ctor
        /// </summary>
        public BlockWorkUserAdapter(IBlockWorkService service, IRequestContextAccessor contextAccessor)
        {
            _service = service;

            var currentUser = contextAccessor.RequestContext.CurrentUser.ToApplicationUser();
            _companyId = currentUser.CompanyId;
        }

        public Task Delete(long id) => _service.Delete(id);

        public Task<long> Add(BlockWorkAddModel model) => _service.Add(model);

        public Task<IEnumerable<BlockWorkModelSimple>> List()=> _service.List();
        
        public Task<BlockWorkModel> Get(long id)=> _service.Get(id);
    }
}