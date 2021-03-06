﻿using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class ZipFileServiceTests
    {
        [Fact]
        public async Task Retrieve()
        {
            var cancellationToken = CancellationToken.None;

            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var fileServiceMock = new Mock<IFileService>();
            var loggerMock = new Mock<ILogger>();
            var fileNameKey = "FileName";
            var containerKey = "containerKey";

            Stream stream = new MemoryStream();
            fileServiceMock.Setup(s => s.OpenWriteStreamAsync(fileNameKey, containerKey, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();
            jsonSerializationServiceMock.Setup(s => s.Serialize(It.IsAny<object>(), It.IsAny<Stream>())).Verifiable();

            var service = NewService(jsonSerializationServiceMock.Object, fileServiceMock.Object, loggerMock.Object);

            await service.SaveCollectionZipAsync(
                fileNameKey,
                containerKey,
                new MetaData(),
                new DevolvedPostcodes(),
                new List<Employer>(),
                new List<EPAOrganisation>(),
                new List<LARSFrameworkDesktop>(),
                new List<LARSFrameworkAimDesktop>(),
                new List<LARSLearningDelivery>(),
                new List<LARSStandard>(),
                new List<Organisation>(),
                new List<Postcode>(),
                cancellationToken);

            fileServiceMock.VerifyAll();
            jsonSerializationServiceMock.VerifyAll();
        }

        private ZipFileService NewService(
            IJsonSerializationService jsonSerializationService = null,
            IFileService fileService = null,
            ILogger logger = null)
        {
            return new ZipFileService(jsonSerializationService, fileService, logger);
        }
    }
}
