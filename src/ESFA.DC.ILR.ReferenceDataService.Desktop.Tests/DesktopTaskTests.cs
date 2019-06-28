﻿using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
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

            var desktopContext = new Mock<IDesktopContext>();
            var referenceDataContext = new ReferenceDataJobContextMessageContext(desktopContext.Object);

            var messageProviderMock = new Mock<IMessageProvider>();
            var referenceDataPopulationServiceMock = new Mock<IReferenceDataPopulationService>();
            var filePersisterMock = new Mock<IFilePersister>();
            var loggerMock = new Mock<ILogger>();

            IMessage message = new TestMessage();
            var referenceDataRoot = new ReferenceDataRoot();

            messageProviderMock.Setup(p => p.ProvideAsync(It.IsAny<IReferenceDataContext>(), cancellationToken)).Returns(Task.FromResult(message)).Verifiable();
            referenceDataPopulationServiceMock.Setup(s => s.PopulateAsync(message, cancellationToken)).Returns(Task.FromResult(referenceDataRoot)).Verifiable();
            filePersisterMock.Setup(s => s.StoreAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, referenceDataRoot, false, cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var task = NewTask(messageProviderMock.Object, referenceDataPopulationServiceMock.Object, filePersisterMock.Object, loggerMock.Object);

            await task.ExecuteAsync(desktopContext.Object, cancellationToken);

            messageProviderMock.VerifyAll();
            referenceDataPopulationServiceMock.VerifyAll();
            filePersisterMock.VerifyAll();
        }

        private ReferenceDataServiceDesktopTask NewTask(
            IMessageProvider messageProvider = null,
            IReferenceDataPopulationService referenceDataPopulationService = null,
            IFilePersister filePersister = null,
            ILogger logger = null)
        {
            return new ReferenceDataServiceDesktopTask(messageProvider, referenceDataPopulationService, filePersister, logger);
        }
    }
}
