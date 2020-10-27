using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Service.Clients;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class EdrsApiService : IEdrsApiService
    {
        private readonly IEmpIdMapper _empIdMapper;
        private readonly IEDRSClientService _edrsClientService;
        private readonly ILogger _logger;

        public EdrsApiService(
            IEmpIdMapper empIdMapper,
            IEDRSClientService edrsClientService,
            ILogger logger)
        {
            _empIdMapper = empIdMapper;
            _edrsClientService = edrsClientService;
            _logger = logger;
        }

        public async Task<List<int>> ValidateErnsAsync(IMessage message, CancellationToken cancellationToken)
        {
            var erns = _empIdMapper.MapEmpIdsFromMessage(message);

            var validErns = new List<int>();

            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                var invalidErns = new HashSet<int>(await _edrsClientService.GetInvalidErns(erns, cancellationToken));
                validErns = erns.Except(invalidErns).ToList();
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            _logger.LogInfo("EDRS API call took: " + stopwatch.Elapsed.TotalSeconds + " secs");

            return validErns;
        }
    }
}