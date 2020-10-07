using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.Internal;
using ESFA.DC.ILR.ReferenceDataService.Service.Mappers;
using ESFA.DC.Logging.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class DesktopReferenceDataSummaryFileServiceTests
    {
        [Fact]
        public async Task ProcessAync()
        {
            var cancellationToken = CancellationToken.None;
            var container = "Container";

            var expectedFileName = @"FISReferenceData\1\FISReferenceData-SummaryReport-202008010900.csv";

            var dateTimeUtc = new System.DateTime(2020, 8, 1, 10, 0, 0);
            var dateTimeUk = new System.DateTime(2020, 8, 1, 9, 0, 0);

            var stats = new List<ReferenceDataSummaryStatistics>
            {
                new ReferenceDataSummaryStatistics { DataSource = "Source1", NumberOfRecords = 1 },
                new ReferenceDataSummaryStatistics { DataSource = "Source2", NumberOfRecords = 2 },
                new ReferenceDataSummaryStatistics { DataSource = "Source3", NumberOfRecords = 3 },
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.ConvertUtcToUk(dateTimeUtc)).Returns(dateTimeUk);

            var csvFileService = new Mock<ICsvFileService>();
            csvFileService.Setup(x => x.WriteAsync<ReferenceDataSummaryStatistics, ReferenceDataSummaryFileMapper>(stats, expectedFileName, container, cancellationToken, null, null)).Returns(Task.CompletedTask);

            var referenceDataStatisticsService = new Mock<IReferenceDataStatisticsService>();
            referenceDataStatisticsService.Setup(x => x.GetStatistics()).Returns(stats);

            var context = new Mock<IReferenceDataContext>();
            context.Setup(x => x.SubmissionDateTimeUTC).Returns(dateTimeUtc);
            context.Setup(x => x.CollectionName).Returns("FISReferenceData");
            context.Setup(x => x.Container).Returns(container);
            context.Setup(x => x.JobId).Returns(1);

            await NewService(csvFileService.Object, referenceDataStatisticsService.Object, dateTimeProviderMock.Object).ProcessAync(context.Object, cancellationToken);

            csvFileService.VerifyAll();
        }

        private DesktopReferenceDataSummaryFileService NewService(
            ICsvFileService csvFileService,
            IReferenceDataStatisticsService referenceDataStatisticsService,
            IDateTimeProvider dateTimeProvider)
        {
            return new DesktopReferenceDataSummaryFileService(csvFileService, Mock.Of<ILogger>(), referenceDataStatisticsService, dateTimeProvider);
        }
    }
}
