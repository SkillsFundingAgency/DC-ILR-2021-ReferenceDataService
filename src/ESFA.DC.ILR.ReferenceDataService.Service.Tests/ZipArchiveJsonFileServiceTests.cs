using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ESFA.DC.ILR.ReferenceDataService.Service.Tests.Models;
using ESFA.DC.Serialization.Json;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests
{
    public class ZipArchiveJsonFileServiceTests
    {
        [Fact]
        public void RetrieveModel()
        {
            var expectedModel = new TestModel
            {
                Id = 1,
                Amount = 1,
                Name = "One",
            };

            using (var stream = BuildStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    NewService().RetrieveModel<TestModel>(archive, "Single.Json").Should().BeEquivalentTo(expectedModel);
                }
            }
        }

        [Fact]
        public void RetrieveModels()
        {
            var expectedModels = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Amount = 1,
                    Name = "One",
                },
                new TestModel
                {
                    Id = 2,
                    Amount = 2,
                    Name = "Two",
                },
                new TestModel
                {
                    Id = 3,
                    Amount = 3,
                    Name = "Three",
                },
            };

            using (var stream = BuildStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    NewService().RetrieveModels<TestModel>(archive, "Collection.Json").Should().BeEquivalentTo(expectedModels);
                }
            }
        }

        [Fact]
        public void RetrieveModels_WithFilter()
        {
            var expectedModels = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Amount = 1,
                    Name = "One",
                },
            };

            using (var stream = BuildStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    NewService().RetrieveModels<TestModel>(archive, "Collection.Json", x => x.Id == 1).Should().BeEquivalentTo(expectedModels);
                }
            }
        }

        private Stream BuildStream()
        {
            return File.OpenRead(@"TestFiles\ZipArchive.zip") as Stream;
        }

        private ZipArchiveJsonFileService NewService()
        {
            return new ZipArchiveJsonFileService(new JsonSerializationService());
        }
    }
}