using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Config;
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
            var container = "Container";
            var collectionName = "Collection";
            var filePrefix = "FISReferenceData";
            var submissionDateTime = new DateTime(2020, 01, 01, 9, 00, 00);
            var cancellationToken = CancellationToken.None;
            var zipFileServiceMock = new Mock<IZipFileService>();
            var loggerMock = new Mock<ILogger>();
            var fileNameServiceMock = new Mock<IDesktopReferenceDataFileNameService>();
            var desktopRefDataConfigMock = new Mock<IDesktopReferenceDataConfiguration>();

            var context = new Mock<IReferenceDataContext>();
            context.Setup(x => x.Container).Returns(container);
            context.Setup(x => x.JobId).Returns(1);
            context.Setup(x => x.CollectionName).Returns(collectionName);
            context.Setup(x => x.SubmissionDateTimeUTC).Returns(submissionDateTime);

            var filePath = $@"{context.Object.CollectionName}\{context.Object.JobId}";
            var outputFileName = $@"{filePath}/FISReferenceData.2.zip";

            fileNameServiceMock.Setup(fsm => fsm.BuildFileName(filePath, filePrefix, "2")).Returns(outputFileName);

            var desktopReferenceDataRoot = new DesktopReferenceDataRoot();

            zipFileServiceMock.Setup(s => s.SaveCollectionZipAsync(
                outputFileName,
                container,
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

            desktopRefDataConfigMock.Setup(x => x.DesktopReferenceDataFilePreFix).Returns(filePrefix);

            var service = NewService(desktopRefDataConfigMock.Object, fileNameServiceMock.Object, zipFileServiceMock.Object, loggerMock.Object);

            await service.ProcessAsync(context.Object, desktopReferenceDataRoot, cancellationToken);

            desktopRefDataConfigMock.VerifyAll();
            fileNameServiceMock.VerifyAll();
            zipFileServiceMock.VerifyAll();
        }

        private DesktopReferenceDataFileService NewService(
            IDesktopReferenceDataConfiguration desktopConfig = null,
            IDesktopReferenceDataFileNameService fileNameService = null,
            IZipFileService zipFileService = null,
            ILogger logger = null)
        {
            return new DesktopReferenceDataFileService(desktopConfig, fileNameService, zipFileService, logger);
        }
    }
}
