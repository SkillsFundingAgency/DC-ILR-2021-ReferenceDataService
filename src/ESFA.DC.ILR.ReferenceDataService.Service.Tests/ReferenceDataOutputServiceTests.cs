using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Serialization.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class ReferenceDataOutputServiceTests
    {
        [Fact]
        public async void OutputAsync()
        {
            var cancellationToken = CancellationToken.None;

            var container = "Container";
            var referenceDataOutputKey = "ReferenceDataOutputKey";

            var referenceDataRoot = new ReferenceDataRoot();
            Stream stream = new MemoryStream();

            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var fileServiceMock = new Mock<IFileService>();

            fileServiceMock.Setup(s => s.OpenWriteStreamAsync(referenceDataOutputKey, container, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();

            referenceDataContextMock.SetupGet(c => c.Container).Returns(container);
            referenceDataContextMock.SetupGet(c => c.OutputReferenceDataFileKey).Returns(referenceDataOutputKey);

            jsonSerializationServiceMock.Setup(s => s.Serialize(referenceDataRoot, stream)).Verifiable();

            await NewService(jsonSerializationServiceMock.Object, fileServiceMock.Object)
                .OutputAsync(referenceDataContextMock.Object, referenceDataRoot, cancellationToken);

            jsonSerializationServiceMock.VerifyAll();
            fileServiceMock.VerifyAll();
        }

        private ReferenceDataOutputService NewService(IJsonSerializationService jsonSerializationService = null, IFileService fileService = null)
        {
            return new ReferenceDataOutputService(jsonSerializationService, fileService);
        }
    }
}
