using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Mappers;
using ESFA.DC.ILR.ReferenceDataService.Service.Model;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class DesktopReferenceDataSummaryFileService : IDesktopReferenceDataSummaryFileService
    {
        private readonly ICsvFileService _csvFileService;
        private readonly ILogger _logger;
        private readonly IReferenceDataStatisticsService _referenceDataStatisticsService;

        public DesktopReferenceDataSummaryFileService(
            ICsvFileService csvFileService,
            ILogger logger,
            IReferenceDataStatisticsService referenceDataStatisticsService)
        {
            _csvFileService = csvFileService;
            _logger = logger;
            _referenceDataStatisticsService = referenceDataStatisticsService;
        }

        public async Task ProcessAync(string container, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Generating Reference Data Report Summary");
            var statistics = (IEnumerable<DesktopReferenceDataSummaryReport>)_referenceDataStatisticsService.GetStatistics();
            await _csvFileService.WriteAsync<DesktopReferenceDataSummaryReport, ReferenceDataSummaryFileMapper>(statistics, "lkasdj", container, cancellationToken);
        }
    }
}
