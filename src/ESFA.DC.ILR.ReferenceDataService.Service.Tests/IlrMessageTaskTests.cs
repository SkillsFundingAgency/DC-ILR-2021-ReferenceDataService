using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.Logging.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class IlrMessageTaskTests
    {
        [Fact]
        public async Task Retrieve()
        {
            var cancellationToken = CancellationToken.None;

            var referenceDataContextMock = new Mock<IReferenceDataContext>();

            var messageProviderMock = new Mock<IMessageProvider>();
            var referenceDataPopulationServiceMock = new Mock<IReferenceDataPopulationService>();
            var gZipFIleProviderMock = new Mock<IGzipFileProvider>();
            var loggerMock = new Mock<ILogger>();

            var message = new Message();
            var referenceDataRoot = new ReferenceDataRoot();

            messageProviderMock.Setup(p => p.ProvideAsync(referenceDataContextMock.Object, cancellationToken)).Returns(Task.FromResult(message)).Verifiable();
            referenceDataPopulationServiceMock.Setup(s => s.PopulateAsync(message, cancellationToken)).Returns(Task.FromResult(referenceDataRoot)).Verifiable();
            gZipFIleProviderMock.Setup(s => s.CompressAndStoreAsync(referenceDataContextMock.Object, referenceDataRoot, cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var service = NewService(messageProviderMock.Object, referenceDataPopulationServiceMock.Object, gZipFIleProviderMock.Object, loggerMock.Object);

            await service.ExecuteAsync(referenceDataContextMock.Object, cancellationToken);

            messageProviderMock.VerifyAll();
            referenceDataPopulationServiceMock.VerifyAll();
            gZipFIleProviderMock.VerifyAll();
        }

        private IlrMessageTask NewService(
            IMessageProvider messageProvider = null,
            IReferenceDataPopulationService referenceDataPopulationService = null,
            IGzipFileProvider gZipFileProvider = null,
            ILogger logger = null)
        {
            return new IlrMessageTask(messageProvider, referenceDataPopulationService, gZipFileProvider, logger);
        }
    }
}
