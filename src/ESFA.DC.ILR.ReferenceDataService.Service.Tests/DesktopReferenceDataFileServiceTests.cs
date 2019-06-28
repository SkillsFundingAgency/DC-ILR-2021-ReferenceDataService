using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class DesktopReferenceDataFileServiceTests
    {
        [Fact]
        public async Task Retrieve()
        {
            var cancellationToken = CancellationToken.None;
            var jsonString = "json string";

            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var zipFileServiceMock = new Mock<IZipFileService>();
            var loggerMock = new Mock<ILogger>();

            var desktopReferenceDataRoot = new DesktopReferenceDataRoot();

            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<MetaData>())).Returns(jsonString).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<IReadOnlyCollection<Employer>>())).Returns(jsonString).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<IReadOnlyCollection<EPAOrganisation>>())).Returns(jsonString).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<IReadOnlyCollection<LARSFramework>>())).Returns(jsonString).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<IReadOnlyCollection<LARSLearningDelivery>>())).Returns(jsonString).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<IReadOnlyCollection<LARSStandard>>())).Returns(jsonString).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<IReadOnlyCollection<Organisation>>())).Returns(jsonString).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<IReadOnlyCollection<Postcode>>())).Returns(jsonString).Verifiable();

            zipFileServiceMock.Setup(s => s.SaveCollectionZipAsync(
                referenceDataContextMock.Object.OutputReferenceDataFileKey,
                referenceDataContextMock.Object.Container,
                It.IsAny<IReadOnlyDictionary<string, string>>(),
                cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var service = NewService(jsonSerializationServiceMock.Object, zipFileServiceMock.Object, loggerMock.Object);

            await service.ProcessAync(referenceDataContextMock.Object, desktopReferenceDataRoot, cancellationToken);

            jsonSerializationServiceMock.VerifyAll();
            zipFileServiceMock.VerifyAll();
        }

        private DesktopReferenceDataFileService NewService(
            IJsonSerializationService jsonSerializationService = null,
            IZipFileService zipFileService = null,
            ILogger logger = null)
        {
            return new DesktopReferenceDataFileService(jsonSerializationService, zipFileService, logger);
        }
    }
}
