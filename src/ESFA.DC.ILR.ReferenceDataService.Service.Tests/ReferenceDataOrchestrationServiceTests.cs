using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class ReferenceDataOrchestrationServiceTests
    {
        [Fact]
        public void Process()
        {
            var cancellationToken = CancellationToken.None;
            var referenceDataContextMock = new Mock<IReferenceDataContext>();

            var taskProviderMock = new Mock<IIlrMessageTaskProvider>();
            var loggerMock = new Mock<ILogger>();

            taskProviderMock.Setup(p => p.Provide(referenceDataContextMock.Object, cancellationToken)).Returns(Task.FromResult(default(object))).Verifiable();

            var result = NewService(taskProviderMock.Object, loggerMock.Object).Process(referenceDataContextMock.Object, cancellationToken);

            result.Status.ToString().Should().Be("RanToCompletion");
        }

        private ReferenceDataOrchestrationService NewService(IIlrMessageTaskProvider taskProvider = null, ILogger logger = null)
        {
            return new ReferenceDataOrchestrationService(taskProvider, logger);
        }
    }
}
