using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tests.Mappers;
using ESFA.DC.ILR.ReferenceDataService.Service.Tests.Models;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class CsvRetrievalServiceTests
    {
        [Fact]
        public async Task RetrieveCsvData()
        {
            var cancellationToken = CancellationToken.None;

            var fileServiceMock = new Mock<IFileService>();
            var fileNameKey = "FileName";
            var containerKey = "containerKey";

            Stream stream = new MemoryStream();
            fileServiceMock.Setup(s => s.OpenReadStreamAsync(fileNameKey, containerKey, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();

            var service = NewService(fileServiceMock.Object);

            await service.RetrieveCsvData<TestModel, TestMapper>(fileNameKey, containerKey, cancellationToken);

            fileServiceMock.VerifyAll();
        }

        private CsvRetrievalService NewService(IFileService fileService = null)
        {
            return new CsvRetrievalService(fileService);
        }
    }
}
