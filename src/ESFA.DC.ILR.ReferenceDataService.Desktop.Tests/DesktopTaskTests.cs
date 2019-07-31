using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.Logging.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class DesktopTaskTests
    {
        [Fact]
        public async Task ExecuteAsync()
        {
            var cancellationToken = CancellationToken.None;

            IReadOnlyCollection<ReturnPeriod> returnPeriods = new List<ReturnPeriod>
            {
                new ReturnPeriod { Name = "R01", Period = 1, Start = new DateTime(2019, 08, 23), End = new DateTime(2019, 09, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R02", Period = 2, Start = new DateTime(2019, 09, 07), End = new DateTime(2019, 10, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R03", Period = 3, Start = new DateTime(2019, 10, 05), End = new DateTime(2019, 11, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R04", Period = 4, Start = new DateTime(2019, 11, 07), End = new DateTime(2019, 12, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R05", Period = 5, Start = new DateTime(2019, 12, 07), End = new DateTime(2020, 01, 07, 23, 59, 59) },
                new ReturnPeriod { Name = "R06", Period = 6, Start = new DateTime(2020, 01, 08), End = new DateTime(2020, 02, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R07", Period = 7, Start = new DateTime(2020, 02, 07), End = new DateTime(2020, 03, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R08", Period = 8, Start = new DateTime(2020, 03, 07), End = new DateTime(2020, 04, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R09", Period = 9, Start = new DateTime(2020, 04, 05), End = new DateTime(2020, 05, 07, 23, 59, 59) },
                new ReturnPeriod { Name = "R10", Period = 10, Start = new DateTime(2020, 05, 08), End = new DateTime(2020, 06, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R11", Period = 11, Start = new DateTime(2020, 06, 07), End = new DateTime(2020, 07, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R12", Period = 12, Start = new DateTime(2020, 07, 05), End = new DateTime(2020, 08, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R13", Period = 13, Start = new DateTime(2020, 08, 07), End = new DateTime(2020, 09, 13, 23, 59, 59) },
                new ReturnPeriod { Name = "R14", Period = 14, Start = new DateTime(2020, 09, 14), End = new DateTime(2020, 10, 17, 23, 59, 59) },
            };

            var desktopContextMock = new Mock<IDesktopContext>();
            desktopContextMock.Setup(dm => dm.KeyValuePairs)
               .Returns(new Dictionary<string, object>
               {
                    { "IlrReferenceData", "IlrReferenceData" },
                    { "OriginalFilename", "OriginalFilename" },
                    { "Filename", "Filename" },
                    { "Container", "Container" },
                    { "ReturnPeriod", 0 }
               });

            IReferenceDataContext referenceDataContext = new ReferenceDataJobContextMessageContext(desktopContextMock.Object);

            var messageProviderMock = new Mock<IMessageProvider>();
            var referenceDataPopulationServiceMock = new Mock<IReferenceDataPopulationService>();
            var filePersisterMock = new Mock<IFilePersister>();
            var desktopContextReturnPeriodUpdateServiceMock = new Mock<IDesktopContextReturnPeriodUpdateService>();
            var loggerMock = new Mock<ILogger>();

            IMessage message = new TestMessage
            {
                HeaderEntity = new TestHeader
                {
                    CollectionDetailsEntity = new TestCollectionDetails
                    {
                        FilePreparationDate = new DateTime(2019, 9, 1)
                    }
                }
            };
            var referenceDataRoot = new ReferenceDataRoot
            {
                MetaDatas = new Model.MetaData.MetaData
                {
                    CollectionDates = new IlrCollectionDates
                    {
                        ReturnPeriods = returnPeriods
                    }
                }
            };

            messageProviderMock.Setup(p => p.ProvideAsync(It.IsAny<IReferenceDataContext>(), cancellationToken)).Returns(Task.FromResult(message)).Verifiable();
            referenceDataPopulationServiceMock.Setup(s => s.PopulateAsync(message, cancellationToken)).Returns(Task.FromResult(referenceDataRoot)).Verifiable();
            filePersisterMock.Setup(s => s.StoreAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, referenceDataRoot, false, cancellationToken)).Returns(Task.CompletedTask).Verifiable();
            desktopContextReturnPeriodUpdateServiceMock.Setup(m => m.UpdateCollectionPeriod(It.IsAny<IReferenceDataContext>(), It.IsAny<DateTime>(), referenceDataRoot.MetaDatas.CollectionDates.ReturnPeriods)).Verifiable();

            var task = NewTask(
                messageProviderMock.Object,
                referenceDataPopulationServiceMock.Object,
                filePersisterMock.Object,
                desktopContextReturnPeriodUpdateServiceMock.Object,
                loggerMock.Object);

            await task.ExecuteAsync(desktopContextMock.Object, cancellationToken);

            messageProviderMock.VerifyAll();
            referenceDataPopulationServiceMock.VerifyAll();
            filePersisterMock.VerifyAll();
            desktopContextReturnPeriodUpdateServiceMock.VerifyAll();
        }

        private ReferenceDataServiceDesktopTask NewTask(
            IMessageProvider messageProvider = null,
            IReferenceDataPopulationService referenceDataPopulationService = null,
            IFilePersister filePersister = null,
            IDesktopContextReturnPeriodUpdateService desktopContextReturnPeriodUpdateService = null,
            ILogger logger = null)
        {
            return new ReferenceDataServiceDesktopTask(
                messageProvider,
                referenceDataPopulationService,
                filePersister,
                desktopContextReturnPeriodUpdateService,
                logger);
        }
    }
}
