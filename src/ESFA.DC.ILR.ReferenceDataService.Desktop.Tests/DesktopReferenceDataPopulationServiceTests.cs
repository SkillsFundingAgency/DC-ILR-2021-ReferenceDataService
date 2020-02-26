using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class DesktopReferenceDataPopulationServiceTests
    {
        [Fact]
        public async void PopulateAsync()
        {
            IMessage message = new TestMessage();
            var mapperData = new MapperData();

            var desktopReferenceData = new ReferenceDataRoot();
            var metaData = new MetaData();

            var referenceDataContext = new Mock<IReferenceDataContext>();
            var messageMapperServiceMock = new Mock<IMessageMapperService>();
            var desktopReferenceDataMapperServiceMock = new Mock<IDesktopReferenceDataMapperService>();

            messageMapperServiceMock.Setup(sm => sm.MapFromMessage(message)).Returns(mapperData);
            desktopReferenceDataMapperServiceMock.Setup(sm => sm.MapReferenceData(referenceDataContext.Object, It.IsAny<MapperData>(), CancellationToken.None)).Returns(Task.FromResult(desktopReferenceData));

            var result = await NewService(messageMapperServiceMock.Object,desktopReferenceDataMapperServiceMock.Object).PopulateAsync(referenceDataContext.Object, message, CancellationToken.None);

            result.Should().BeEquivalentTo(desktopReferenceData);
        }

        private DesktopReferenceDataPopulationService NewService(
            IMessageMapperService messageMapperService = null,
            IDesktopReferenceDataMapperService desktopReferenceDataMapperService = null)
        {
            return new DesktopReferenceDataPopulationService(
                messageMapperService,
                desktopReferenceDataMapperService,
                Mock.Of<ILogger>());
        }
    }
}
