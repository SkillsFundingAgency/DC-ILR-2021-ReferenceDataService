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
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.Logging.Interfaces;
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
            var zipFileServiceMock = new Mock<IZipFileService>();
            var loggerMock = new Mock<ILogger>();

            var desktopReferenceDataRoot = new DesktopReferenceDataRoot();

            zipFileServiceMock.Setup(s => s.SaveCollectionZipAsync(
                referenceDataContextMock.Object.OutputReferenceDataFileKey,
                referenceDataContextMock.Object.Container,
                It.IsAny<MetaData>(),
                It.IsAny<DevolvedPostcodes>(),
                It.IsAny<IReadOnlyCollection<Employer>>(),
                It.IsAny<IReadOnlyCollection<EPAOrganisation>>(),
                It.IsAny<IReadOnlyCollection<LARSFrameworkDesktop>>(),
                It.IsAny<IReadOnlyCollection<LARSFrameworkAimDesktop>>(),
                It.IsAny<IReadOnlyCollection<LARSLearningDelivery>>(),
                It.IsAny<IReadOnlyCollection<LARSStandard>>(),
                It.IsAny<IReadOnlyCollection<Organisation>>(),
                It.IsAny<IReadOnlyCollection<Postcode>>(),
                cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var service = NewService(zipFileServiceMock.Object, loggerMock.Object);

            await service.ProcessAync(referenceDataContextMock.Object, desktopReferenceDataRoot, cancellationToken);

            zipFileServiceMock.VerifyAll();
        }

        private DesktopReferenceDataFileService NewService(
            IZipFileService zipFileService = null,
            ILogger logger = null)
        {
            return new DesktopReferenceDataFileService(zipFileService, logger);
        }
    }
}
