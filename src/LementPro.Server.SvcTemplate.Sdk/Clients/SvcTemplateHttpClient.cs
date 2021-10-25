using System;
using System.Net.Http;
using System.Threading.Tasks;
using LementPro.Server.Common.Sdk.Http.Abstract;
using LementPro.Server.SvcTemplate.Sdk.Abstract;

namespace LementPro.Server.SvcTemplate.Sdk.Clients
{
    /// <inheritdoc cref="ISvcTemplateHttpClient"/>
    public class SvcTemplateHttpClient : ISvcTemplateHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ISdkHttpMessageService _httpMessageService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="httpMessageService"></param>
        public SvcTemplateHttpClient(HttpClient httpClient, ISdkHttpMessageService httpMessageService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpMessageService = httpMessageService ?? throw new ArgumentNullException(nameof(httpMessageService)); ;
        }

        ///<inheritdoc />
        public async Task<T200> SendAsync<T200, T422>(HttpMethod method, string uri, object data = null)
        {
            var message = _httpMessageService.CreateRequestMessage(method, uri, data);
            var response = await _httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            return await _httpMessageService.ReadResponseMessage<T200, T422>(response);
        }
    }
}
