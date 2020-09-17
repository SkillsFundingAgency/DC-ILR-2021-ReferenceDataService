using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.Logging.Interfaces;
using Flurl;
using Flurl.Http;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Clients
{
    public class ClientService
    {
        private readonly ILogger _logger;

        public ClientService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResult> PostAsync<TContent, TResult>(string url, TContent content)
        {
            try
            {
                return await url
                    .PostJsonAsync(content)
                    .ReceiveJson<TResult>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<TResult> GetAsync<TResult>(string url, IDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                url.SetQueryParam(parameter.Key, parameter.Value);
            }

            return await url.GetJsonAsync<TResult>();
        }
    }
}