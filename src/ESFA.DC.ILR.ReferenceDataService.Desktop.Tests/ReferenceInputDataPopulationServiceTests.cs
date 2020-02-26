using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class ReferenceInputDataPopulationServiceTests
    {
        [Fact]
        public async void PopulateAsync()
        {
            var desktopReferenceData = new DesktopReferenceDataRoot();

            var referenceDataContext = new Mock<IReferenceDataContext>();
            var mapperServiceMock = new Mock<IReferenceInputDataMapperService>();
            mapperServiceMock.Setup(ms => ms.MapReferenceData(referenceDataContext.Object, CancellationToken.None)).Returns(Task.FromResult(desktopReferenceData));

            var result = await NewService(mapperServiceMock.Object).PopulateAsync(referenceDataContext.Object, CancellationToken.None);

            result.Should().BeEquivalentTo(desktopReferenceData);
        }

        private ReferenceInputDataPopulationService NewService(IReferenceInputDataMapperService mapperService = null)
        {
            return new ReferenceInputDataPopulationService(
                mapperService,
                Mock.Of<ILogger>());
        }
    }
}
