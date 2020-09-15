using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Service.Clients;
using ESFA.DC.Logging.Interfaces;
using Flurl;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Clients
{
    public class EDRSClientService : ClientService, IEDRSClientService
    {
        private readonly IClientService _clientService;
        protected string Url;
        
        public EDRSClientService(
            ILogger logger,
            ApiSettings settings,
            IClientService clientService) 
            : base(logger)
        {
            _clientService = clientService;
            Url = settings.EDRSApiBaseUrl.AppendPathSegment("edrs");
        }

        public async Task<IEnumerable<int>> ValidateErns(IReadOnlyCollection<int> erns, CancellationToken cancellationToken)
        {
            var url = Url.AppendPathSegment("validate");
            var response = await _clientService.PostAsync<IEnumerable<int>, IEnumerable<int>>(url, erns);

            return response;
        }
    }
}