using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.Internal;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Mappers;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class DesktopReferenceDataSummaryFileService : IDesktopReferenceDataSummaryFileService
    {
        private readonly ICsvFileService _csvFileService;
        private readonly ILogger _logger;
        private readonly IReferenceDataStatisticsService _referenceDataStatisticsService;
        private readonly IDateTimeProvider _dateTimeProvider;

        private string filePrefix = "{0}-SummaryReport-{1}.csv";

        public DesktopReferenceDataSummaryFileService(
            ICsvFileService csvFileService,
            ILogger logger,
            IReferenceDataStatisticsService referenceDataStatisticsService,
            IDateTimeProvider dateTimeProvider)
        {
            _csvFileService = csvFileService;
            _logger = logger;
            _referenceDataStatisticsService = referenceDataStatisticsService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task ProcessAync(IReferenceDataContext context, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Generating Reference Data Report Summary");

            var ukDateTime = _dateTimeProvider.ConvertUtcToUk(context.SubmissionDateTimeUTC);

            var filePath = BuildFilePath(context);
            var fileName = FileNameBuilder(context.CollectionName, filePath, ukDateTime);

            var statistics = _referenceDataStatisticsService.GetStatistics();
            await _csvFileService.WriteAsync<ReferenceDataSummaryStatistics, ReferenceDataSummaryFileMapper>(statistics, fileName, context.Container, cancellationToken);
        }

        private string FileNameBuilder(string prefix, string filePath, DateTime dateTime) =>
            Path.Combine(filePath, string.Format(filePrefix, prefix, dateTime.ToString("yyyyMMddHHmm")));

        private string BuildFilePath(IReferenceDataContext context)
        {
            return $@"{context.CollectionName}\{context.JobId}";
        }
    }
}
