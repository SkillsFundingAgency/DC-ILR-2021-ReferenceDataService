using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class MessageProviderTests
    {
        [Fact]
        public async Task Provide()
        {
            var cancellationToken = CancellationToken.None;

            var fileReference = "FileReference";
            var container = "Container";

            Stream stream = new MemoryStream();

            var message = new Message();

            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var fileServiceMock = new Mock<IFileService>();
            var xmlSerializationServiceMock = new Mock<IXmlSerializationService>();

            referenceDataContextMock.SetupGet(c => c.FileReference).Returns(fileReference);
            referenceDataContextMock.SetupGet(c => c.Container).Returns(container);

            fileServiceMock.Setup(s => s.OpenReadStreamAsync(fileReference, container, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();
            xmlSerializationServiceMock.Setup(s => s.Deserialize<Message>(stream)).Returns(message).Verifiable();

            var providedMessage = await NewProvider(fileServiceMock.Object, xmlSerializationServiceMock.Object).ProvideAsync(referenceDataContextMock.Object, cancellationToken);

            providedMessage.Should().Be(message);

            fileServiceMock.VerifyAll();
            xmlSerializationServiceMock.VerifyAll();
        }

        private MessageProvider NewProvider(IFileService fileService = null, IXmlSerializationService xmlSerializationService = null)
        {
            return new MessageProvider(fileService, xmlSerializationService);
        }
    }
}
