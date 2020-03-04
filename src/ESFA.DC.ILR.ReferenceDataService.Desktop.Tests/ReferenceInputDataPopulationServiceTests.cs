using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;
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

            var inputReferenceDataContext = new Mock<IInputReferenceDataContext>();
            var mapperServiceMock = new Mock<IReferenceInputDataMapperService>();
            mapperServiceMock.Setup(ms => ms.MapReferenceData(inputReferenceDataContext.Object, CancellationToken.None)).Returns(Task.FromResult(desktopReferenceData));

            var result = await NewService(mapperServiceMock.Object).PopulateAsync(inputReferenceDataContext.Object, CancellationToken.None);

            result.Should().BeTrue();
        }

        private ReferenceInputDataPopulationService NewService(IReferenceInputDataMapperService mapperService = null)
        {
            var referenceInputTruncatorMock = new Mock<IReferenceInputTruncator>();
            referenceInputTruncatorMock
                .Setup(s => s.TruncateReferenceDataAsync(It.IsAny<IInputReferenceDataContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var referenceInputPersistence = new Mock<IReferenceInputPersistence>();
            referenceInputPersistence
                .Setup(s => s.PersistEFModelsAsync(It.IsAny<IInputReferenceDataContext>(), It.IsAny<IEFReferenceInputDataRoot>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);


            return new ReferenceInputDataPopulationService(
                mapperService,
                Mock.Of<ReferenceInputEFMapper>(),
                Mock.Of<EFModelIdentityAssigner>(),
                referenceInputTruncatorMock.Object,
                referenceInputPersistence.Object,
                Mock.Of<ILogger>());
        }
    }
}
