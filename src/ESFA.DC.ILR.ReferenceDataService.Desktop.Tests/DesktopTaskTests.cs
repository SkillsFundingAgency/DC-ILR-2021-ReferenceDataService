using System.Threading;
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
            var gZipFIleProviderMock = new Mock<IFileProvider>();
            var loggerMock = new Mock<ILogger>();

            IMessage message = new TestMessage();
            var referenceDataRoot = new ReferenceDataRoot();

            messageProviderMock.Setup(p => p.ProvideAsync(It.IsAny<IReferenceDataContext>(), cancellationToken)).Returns(Task.FromResult(message)).Verifiable();
            referenceDataPopulationServiceMock.Setup(s => s.PopulateAsync(message, cancellationToken)).Returns(Task.FromResult(referenceDataRoot)).Verifiable();
            gZipFIleProviderMock.Setup(s => s.StoreAsync(It.IsAny<IReferenceDataContext>(), referenceDataRoot, false, cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var task = NewTask(messageProviderMock.Object, referenceDataPopulationServiceMock.Object, gZipFIleProviderMock.Object, loggerMock.Object);

            await task.ExecuteAsync(desktopContext.Object, cancellationToken);

            messageProviderMock.VerifyAll();
            referenceDataPopulationServiceMock.VerifyAll();
            gZipFIleProviderMock.VerifyAll();
        }

        private ReferenceDataServiceDesktopTask NewTask(
            IMessageProvider messageProvider = null,
            IReferenceDataPopulationService referenceDataPopulationService = null,
            IFileProvider gZipFileProvider = null,
            ILogger logger = null)
        {
            return new ReferenceDataServiceDesktopTask(messageProvider, referenceDataPopulationService, gZipFileProvider, logger);
        }
    }
}
