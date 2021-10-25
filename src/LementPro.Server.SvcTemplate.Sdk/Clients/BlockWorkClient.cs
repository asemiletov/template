using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LementPro.Server.Common.Sdk.Models;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;

namespace LementPro.Server.SvcTemplate.Sdk.Clients
{
    /// <inheritdoc cref="IBlockWorkUserAdapter" />
    public class BlockWorkClient : IBlockWorkUserAdapter
    {
        private readonly ISvcTemplateHttpClient _httpClient;

        /// <summary>
        /// .ctor
        /// </summary>
        public BlockWorkClient(ISvcTemplateHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc />
        public async Task<long> Add(BlockWorkAddModel model) =>
            await _httpClient.SendAsync<long, ErrorResponse>(HttpMethod.Post, "/api/blockwork", model);

        /// <inheritdoc />
        public async Task<IEnumerable<BlockWorkModelSimple>> List() =>
            await _httpClient.SendAsync<IEnumerable<BlockWorkModelSimple>, ErrorResponse>(HttpMethod.Get, "/api/blockwork");

        /// <inheritdoc />
        public async Task<BlockWorkModel> Get(long id) =>
            await _httpClient.SendAsync<BlockWorkModel, ErrorResponse>(HttpMethod.Get, $"/api/blockwork/{id}");

        /// <inheritdoc />
        public async Task Delete(long id) =>
            await _httpClient.SendAsync<object, ErrorResponse>(HttpMethod.Delete, $"/api/blockwork/{id}");
    }
}
