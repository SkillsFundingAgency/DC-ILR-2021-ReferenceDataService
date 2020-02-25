using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class DesktopReferenceDataFileServiceTests
    {
        [Fact]
        public async Task ProcessAync()
        {
            var outputFileName = "desktop/1920/referencedata/FISReferenceData_0.1.0.zip";
            var cancellationToken = CancellationToken.None;
            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var zipFileServiceMock = new Mock<IZipFileService>();
            var loggerMock = new Mock<ILogger>();
            var fileNameServiceMock = new Mock<IDesktopReferenceDataFileNameService>();

            fileNameServiceMock.Setup(fsm => fsm.BuildFileName(It.IsAny<string>(), It.IsAny<string>())).Returns(outputFileName);

            var desktopReferenceDataRoot = new DesktopReferenceDataRoot();

          //  var rdsModelVersion = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "ESFA.DC.ILR.ReferenceDataService.Model").Version.ToString(3);

          //  referenceDataContextMock.Setup(d => d.DesktopReferenceDataStoragePath).Returns(filePath);

            zipFileServiceMock.Setup(s => s.SaveCollectionZipAsync(
                outputFileName,
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

            var service = NewService(fileNameServiceMock.Object, zipFileServiceMock.Object, loggerMock.Object);

            await service.ProcessAync(referenceDataContextMock.Object, desktopReferenceDataRoot, cancellationToken);

            fileNameServiceMock.VerifyAll();
            zipFileServiceMock.VerifyAll();
        }

        private DesktopReferenceDataFileService NewService(
            IDesktopReferenceDataFileNameService fileNameService = null,
            IZipFileService zipFileService = null,
            ILogger logger = null)
        {
            return new DesktopReferenceDataFileService(fileNameService, zipFileService, logger);
        }
    }
}
