﻿using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Providers.Tests
{
    public class GZipFileProviderTests
    {
        [Fact]
        public async Task CompressAndStoreAsync()
        {
            var cancellationToken = CancellationToken.None;

            var outputReferenceDataFileKey = "FileReference";
            var container = "Container";

            ReferenceDataRoot referenceDataRoot = new ReferenceDataRoot();

            Stream stream = new MemoryStream();

            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var fileServiceMock = new Mock<IFileService>();
            var jsonSerializationServiceeMock = new Mock<IJsonSerializationService>();

            referenceDataContextMock.SetupGet(c => c.OutputReferenceDataFileKey).Returns(outputReferenceDataFileKey);
            referenceDataContextMock.SetupGet(c => c.Container).Returns(container);

            fileServiceMock.Setup(s => s.OpenWriteStreamAsync(referenceDataContextMock.Object.OutputReferenceDataFileKey, referenceDataContextMock.Object.Container, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();
            jsonSerializationServiceeMock.Setup(s => s.Serialize(referenceDataRoot, It.IsAny<GZipStream>())).Verifiable();

            await NewProvider(jsonSerializationServiceeMock.Object, fileServiceMock.Object).CompressAndStoreAsync(referenceDataContextMock.Object, referenceDataRoot, cancellationToken);

            fileServiceMock.VerifyAll();
            jsonSerializationServiceeMock.VerifyAll();
        }

        [Fact]
        public async Task RetrieveAndDecompressAsync()
        {
            var cancellationToken = CancellationToken.None;

            var outputReferenceDataFileKey = "FileReference";
            var container = "Container";

            ReferenceDataRoot referenceDataRoot = new ReferenceDataRoot();

            Stream stream = new MemoryStream();

            var referenceDataContextMock = new Mock<IReferenceDataContext>();
            var fileServiceMock = new Mock<IFileService>();
            var jsonSerializationServiceeMock = new Mock<IJsonSerializationService>();

            referenceDataContextMock.SetupGet(c => c.OutputReferenceDataFileKey).Returns(outputReferenceDataFileKey);
            referenceDataContextMock.SetupGet(c => c.Container).Returns(container);

            fileServiceMock.Setup(s => s.OpenReadStreamAsync(referenceDataContextMock.Object.OutputReferenceDataFileKey, referenceDataContextMock.Object.Container, cancellationToken)).Returns(Task.FromResult(stream)).Verifiable();
            jsonSerializationServiceeMock.Setup(s => s.Deserialize<ReferenceDataRoot>(It.IsAny<GZipStream>())).Verifiable();

            await NewProvider(jsonSerializationServiceeMock.Object, fileServiceMock.Object).RetrieveAndDecompressAsync<ReferenceDataRoot>(referenceDataContextMock.Object, cancellationToken);

            fileServiceMock.VerifyAll();
            jsonSerializationServiceeMock.VerifyAll();
        }

        private GZipFileProvider NewProvider(IJsonSerializationService jsonSerializationService, IFileService fileService = null)
        {
            return new GZipFileProvider(jsonSerializationService, fileService);
        }
    }
}
