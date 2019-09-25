using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.CsvModels;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Mappers;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.Logging.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class ValidationMessagesTaskTests
    {
        [Fact]
        public async Task Execute()
        {
            var cancellationToken = CancellationToken.None;

            var contextMock = new Mock<IReferenceDataContext>();
            contextMock.Setup(cm => cm.ValidationMessagesFileReference).Returns("FileReference");
            contextMock.Setup(cm => cm.Container).Returns("Container");

            var loggerMock = new Mock<ILogger>();
            var csvRetrievalMock = new Mock<ICsvRetrievalService>();
            var validationTransactionMock = new Mock<IValidationMessagesTransaction>();

            IEnumerable<ValidationMessagesModel> models = new List<ValidationMessagesModel>()
            {
                new ValidationMessagesModel()
                {
                    RuleName = "Name",
                    ErrorMessage = "ErrorMessage",
                    Severity = "E",
                    EnabledSLD = true,
                    EnabledDesktop = true,
                },
            };

            csvRetrievalMock
                .Setup(x => x.RetrieveCsvData<ValidationMessagesModel, ValidationMessagesMapper>(contextMock.Object.ValidationMessagesFileReference, contextMock.Object.Container, cancellationToken))
                .Returns(Task.FromResult(models))
                .Verifiable();

            validationTransactionMock
                .Setup(x => x.WriteValidationMessagesAsync(It.IsAny<IEnumerable<Rule>>(), cancellationToken))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await NewService(loggerMock.Object, csvRetrievalMock.Object, validationTransactionMock.Object).ExecuteAsync(contextMock.Object, cancellationToken);

            csvRetrievalMock.VerifyAll();
            validationTransactionMock.VerifyAll();
        }

        private ValidationMessagesTask NewService(
            ILogger logger = null,
            ICsvRetrievalService csvRetrievalService = null,
            IValidationMessagesTransaction validationMessagesTransaction = null)
        {
            return new ValidationMessagesTask(logger, csvRetrievalService, validationMessagesTransaction);
        }
    }
}
