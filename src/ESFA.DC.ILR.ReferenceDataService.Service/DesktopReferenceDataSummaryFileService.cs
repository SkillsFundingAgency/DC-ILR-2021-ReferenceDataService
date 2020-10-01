using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
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

        private string filePrefix = "{0}-SummaryReport-{1}.csv";

        public DesktopReferenceDataSummaryFileService(
            ICsvFileService csvFileService,
            ILogger logger,
            IReferenceDataStatisticsService referenceDataStatisticsService)
        {
            _csvFileService = csvFileService;
            _logger = logger;
            _referenceDataStatisticsService = referenceDataStatisticsService;
        }

        public async Task ProcessAync(IReferenceDataContext context, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Generating Reference Data Report Summary");
            var filePath = BuildFilePath(context);
            var statistics = (List<DesktopReferenceDataSummaryReport>)_referenceDataStatisticsService.GetStatistics();
            await _csvFileService.WriteAsync<DesktopReferenceDataSummaryReport, ReferenceDataSummaryFileMapper>(statistics, FileNameBuilder(context.CollectionName, filePath), context.Container, cancellationToken);
        }

        private string FileNameBuilder(string prefix, string filePath) =>
            Path.Combine(filePath, string.Format(filePrefix, prefix, DateTime.Now.ToString("yyyyMMddHHmm")));

        private string BuildFilePath(IReferenceDataContext context)
        {
            return $@"{context.CollectionName}\{context.JobId}";
        }
    }
}
