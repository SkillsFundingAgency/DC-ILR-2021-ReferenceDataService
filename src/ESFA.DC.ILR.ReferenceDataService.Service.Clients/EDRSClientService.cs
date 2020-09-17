using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Service.Clients;
using ESFA.DC.Logging.Interfaces;
using Flurl;
using Polly;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Clients
{
    public class EDRSClientService : ClientService, IEDRSClientService
    {
        private readonly IAsyncPolicy _retryPolicy;
        protected string Url;
        
        public EDRSClientService(
            IAsyncPolicy retryPolicy,
            ILogger logger,
            ApiSettings settings) 
            : base(logger)
        {
            _retryPolicy = retryPolicy;
            Url = settings.EDRSApiBaseUrl.AppendPathSegment("edrs");
        }

        public async Task<IEnumerable<int>> GetInvalidErns(IReadOnlyCollection<int> erns, CancellationToken cancellationToken)
        {
            var url = Url.AppendPathSegment("validate");

            IEnumerable<int> response = new List<int>();
            await _retryPolicy
                .ExecuteAsync(async () =>
                {
                    response = await PostAsync<IEnumerable<int>, IEnumerable<int>>(url, erns);
                });

            return response;
        }
    }
}