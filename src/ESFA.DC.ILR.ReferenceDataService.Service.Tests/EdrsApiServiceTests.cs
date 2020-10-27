using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Service.Clients;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class EdrsApiServiceTests
    {
        [Fact]
        public async Task ValidateErnsAsyncReturnsValidErns()
        {
            var message = new TestMessage();
            var empIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 31, 32, 33, 34, 35 };

            var loggerMock = new Mock<ILogger>();

            var mapperMock = new Mock<IEmpIdMapper>();
            mapperMock.Setup(m => m.MapEmpIdsFromMessage(message)).Returns(empIds);

            var clientServiceMock = new Mock<IEDRSClientService>();
            clientServiceMock
                .Setup(m => m.GetInvalidErns(empIds, CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<int>());

            var service = NewService(mapperMock.Object, clientServiceMock.Object, loggerMock.Object);
            var result = await service.ValidateErnsAsync(message, CancellationToken.None);

            result.Should().BeEquivalentTo(empIds);
        }

        private EdrsApiService NewService(
            IEmpIdMapper empIdMapper,
            IEDRSClientService edrsClientService,
            ILogger logger)
        {
            return new EdrsApiService(empIdMapper, edrsClientService, logger);
        }
    }
}