﻿using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.Logging.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class DesktopReferenceDataTaskTests
    {
        [Fact]
        public async Task Retrieve()
        {
            var cancellationToken = CancellationToken.None;

            var referenceDataContextMock = new Mock<IReferenceDataContext>();

            var referenceDataPopulationServiceMock = new Mock<IDesktopReferenceDataPopulationService>();
            var desktopReferenceDataFileServiceMock = new Mock<IDesktopReferenceDataFileService>();
            var loggerMock = new Mock<ILogger>();

            var desktopReferenceDataRoot = new DesktopReferenceDataRoot();

            referenceDataPopulationServiceMock.Setup(s => s.PopulateAsync(cancellationToken)).Returns(Task.FromResult(desktopReferenceDataRoot)).Verifiable();
            desktopReferenceDataFileServiceMock.Setup(s => s.ProcessAync(referenceDataContextMock.Object, desktopReferenceDataRoot, cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var service = NewService(referenceDataPopulationServiceMock.Object, desktopReferenceDataFileServiceMock.Object, loggerMock.Object);

            await service.ExecuteAsync(referenceDataContextMock.Object, cancellationToken);

            referenceDataPopulationServiceMock.VerifyAll();
            desktopReferenceDataFileServiceMock.VerifyAll();
        }

        private DesktopReferenceDataTask NewService(
            IDesktopReferenceDataPopulationService referenceDataPopulationService = null,
            IDesktopReferenceDataFileService desktopReferenceDataFileService = null,
            ILogger logger = null)
        {
            return new DesktopReferenceDataTask(referenceDataPopulationService, desktopReferenceDataFileService, logger);
        }
    }
}
