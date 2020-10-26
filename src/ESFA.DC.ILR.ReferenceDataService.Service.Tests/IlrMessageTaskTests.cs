using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.ILR.Tests.Model;
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
            var filePersisterMock = new Mock<IFilePersister>();
            var loggerMock = new Mock<ILogger>();
            var edrsApiServiceMock = new Mock<IEdrsApiService>();

            IMessage message = new TestMessage();
            var referenceDataRoot = new ReferenceDataRoot();

            var featureConfiguration = new FeatureConfiguration
            {
                EDRSAPIEnabled = "false"
            };

            messageProviderMock.Setup(p => p.ProvideAsync(referenceDataContextMock.Object, cancellationToken)).Returns(Task.FromResult(message)).Verifiable();
            referenceDataPopulationServiceMock.Setup(s => s.PopulateAsync(referenceDataContextMock.Object, message, cancellationToken)).Returns(Task.FromResult(referenceDataRoot)).Verifiable();
            filePersisterMock.Setup(s => s.StoreAsync(referenceDataContextMock.Object.OutputIlrReferenceDataFileKey, referenceDataContextMock.Object.Container, referenceDataRoot, false, cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var service = NewService(messageProviderMock.Object, referenceDataPopulationServiceMock.Object, edrsApiServiceMock.Object, filePersisterMock.Object, loggerMock.Object, featureConfiguration);

            await service.ExecuteAsync(referenceDataContextMock.Object, cancellationToken);

            messageProviderMock.VerifyAll();
            referenceDataPopulationServiceMock.VerifyAll();
            filePersisterMock.VerifyAll();
        }

        private IlrMessageTask NewService(
            IMessageProvider messageProvider = null,
            IReferenceDataPopulationService referenceDataPopulationService = null,
            IEdrsApiService edrsApiService = null,
            IFilePersister filePersister = null,
            ILogger logger = null,
            FeatureConfiguration featureConfiguration = null)
        {
            return new IlrMessageTask(messageProvider, referenceDataPopulationService, edrsApiService, filePersister, featureConfiguration, logger);
        }
    }
}
