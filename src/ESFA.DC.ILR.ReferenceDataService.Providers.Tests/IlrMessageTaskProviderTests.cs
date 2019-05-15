using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Context;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Providers.Tests
{
    public class IlrMessageTaskProviderTests
    {
        [Fact]
        public async Task Provide()
        {
            var cancellationToken = CancellationToken.None;

            var taskName = "IlrMessage";
            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var ilrMessageTaskMock = new Mock<IIlrMessageTask>();
            var loggerMock = new Mock<ILogger>();

            referenceDataContextMock.SetupGet(r => r.Tasks).Returns(new List<string> { taskName });

            var result = Task.FromResult(ilrMessageTaskMock.Object);

            var messageTask = NewProvider(ilrMessageTaskMock.Object, loggerMock.Object).Provide(referenceDataContextMock.Object, cancellationToken);

            messageTask.Should().NotBeNull();

            referenceDataContextMock.VerifyAll();
            ilrMessageTaskMock.VerifyAll();
            loggerMock.VerifyAll();
        }

        [Fact]
        public async Task Provide_NoTopic()
        {
            var cancellationToken = CancellationToken.None;

            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var ilrMessageTaskMock = new Mock<IIlrMessageTask>();
            var loggerMock = new Mock<ILogger>();

            referenceDataContextMock.SetupGet(r => r.Tasks).Returns(new List<string>());

            Func<Task> serviceResult = async () =>
            {
               NewProvider(ilrMessageTaskMock.Object, loggerMock.Object).Provide(referenceDataContextMock.Object, cancellationToken);
            };

            serviceResult.Should().Throw<ArgumentException>().WithMessage("Task missing or incorrect from Message Topic.");
        }

        private IlrMessageTaskProvider NewProvider(IIlrMessageTask ilrmessageTask = null, ILogger logger = null)
        {
            return new IlrMessageTaskProvider(ilrmessageTask, logger);
        }
    }
}
