using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LementPro.Server.Common.Exception;
using LementPro.Server.Common.Sdk.Extensions;
using LementPro.Server.Common.Sdk.Models;
using LementPro.Server.SvcTemplate.Common.Enums;
using LementPro.Server.SvcTemplate.Common.Enums.ExCode;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Entities;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IBlockWorkService = LementPro.Server.SvcTemplate.Service.Abstract.IBlockWorkService;

namespace LementPro.Server.SvcTemplate.Service.Concrete
{

    public class BlockWorkService : IBlockWorkService
    {
        private readonly IBlockWorkRepository _repository;
        private readonly ILogger<BlockWorkService> _logger;


        public BlockWorkService(IBlockWorkRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<BlockWorkService>();
        }

        public async Task<long> Add(BlockWorkAddModel model)
        {
            var entity = new BlockWorkEntity()
            {
                Name = model.Name,
                Description = model.Description,
                Status = BlockWorkStatus.New,
                DateCreated = DateTimeOffset.UtcNow,
                DateUpdated = DateTimeOffset.UtcNow,
            };

            await _repository.Add(entity);
            await _repository.SaveAsync();

            _logger.LogInformation($"BlockWork entity id: {entity.Id} created");

            return entity.Id;
        }

        public async Task<IEnumerable<BlockWorkModelSimple>> List()
        {
            var list = await _repository.ToListAsync();
            return list.Select(ConvertToBlockWorkSimpleModel);
        }

        public async Task<BlockWorkModel> Get(long id)
        {
            var entity = await _repository.FindAsync(id);
            if(entity == null)
                throw new ServiceException<ErrorResponse>(HttpStatusCode.NotFound, BlockWorkServiceExCode.NotFound.ToErrorResponse());

            return ConvertToBlockWorkModel(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                _logger.LogInformation($"BlockWork entity id: {id} not found");
                return;
            }

            _repository.Delete(entity);
            await _repository.SaveAsync();

            _logger.LogInformation($"BlockWork entity id: {id} was deleted");
        }

        #region private

        private BlockWorkModelSimple ConvertToBlockWorkSimpleModel(BlockWorkEntity entity)
        {
            return new BlockWorkModelSimple()
            {
                Id = entity.Id,
                Name = entity.Name,
                DateCreated = entity.DateCreated,
                Status = entity.Status
            };
        }

        private BlockWorkModel ConvertToBlockWorkModel(BlockWorkEntity entity)
        {
            return new BlockWorkModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                DateCreated = entity.DateCreated,
                Status = entity.Status,
            };
        }

        #endregion
    }
}
