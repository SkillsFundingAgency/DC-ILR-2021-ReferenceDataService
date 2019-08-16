using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class DevolvedPostcodesRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var cancellationToken = CancellationToken.None;

            IReadOnlyCollection<string> postcodes = new List<string> { "PostCode1", "PostCode2", "PostCode3", "PostCode4" };
            var json = @"[""PostCode1"",""PostCode2"",""PostCode3"",""PostCode4""]";

            var mcaGlataskResult = new TaskCompletionSource<List<McaGlaSofLookup>>();
            var devolvedPostcodesTaskResult = new TaskCompletionSource<List<DevolvedPostcode>>();

            var devolvedPostcodes = new DevolvedPostcodes
            {
                McaGlaSofLookups = new List<McaGlaSofLookup>
                {
                    new McaGlaSofLookup
                    {
                        SofCode = "105",
                        McaGlaFullName = "Full Name 1",
                        McaGlaShortCode = "ShortCode 1",
                        EffectiveFrom = new DateTime(2019, 8, 1)
                    },
                    new McaGlaSofLookup
                    {
                        SofCode = "110",
                        McaGlaFullName = "Full Name 2",
                        McaGlaShortCode = "ShortCode 2",
                        EffectiveFrom = new DateTime(2019, 8, 1)
                    }
                },
                Postcodes = new List<DevolvedPostcode>
                {
                    new DevolvedPostcode
                    {
                        Postcode = "PostCode1",
                        Area = "Area",
                        SourceOfFunding = "105",
                        EffectiveFrom = new DateTime(2019, 8, 1)
                    },
                    new DevolvedPostcode
                    {
                        Postcode = "PostCode2",
                        Area = "Area",
                        SourceOfFunding = "105",
                        EffectiveFrom = new DateTime(2019, 8, 1)
                    },
                    new DevolvedPostcode
                    {
                        Postcode = "PostCode3",
                        Area = "Area",
                        SourceOfFunding = "110",
                        EffectiveFrom = new DateTime(2019, 8, 1)
                    }
                }
            };

            mcaGlataskResult.SetResult(devolvedPostcodes.McaGlaSofLookups);
            devolvedPostcodesTaskResult.SetResult(devolvedPostcodes.Postcodes);

            var postcodesContextMock = new Mock<IPostcodesContext>();
            var postcodesContextFactoryMock = new Mock<IDbContextFactory<IPostcodesContext>>();
            var refdataOptionsMock = new Mock<IReferenceDataOptions>();
            var jsonSerializationMock = new Mock<IJsonSerializationService>();

            postcodesContextFactoryMock.Setup(c => c.Create()).Returns(postcodesContextMock.Object);
            jsonSerializationMock.Setup(sm => sm.Serialize(postcodes)).Returns(json);

            var service = NewServiceMock(postcodesContextFactoryMock.Object, jsonSerializationService: jsonSerializationMock.Object);

            service.Setup(s => s.RetrieveMcaGlaLookups(postcodesContextMock.Object, cancellationToken)).Returns(mcaGlataskResult.Task);
            service.Setup(s => s.RetrieveDevolvedPostcodes(json, cancellationToken)).Returns(devolvedPostcodesTaskResult.Task);

            var serviceResult = await service.Object.RetrieveAsync(postcodes, cancellationToken);

            serviceResult.Should().BeEquivalentTo(devolvedPostcodes);
        }

        private DevolvedPostcodesRepositoryService NewService(
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IReferenceDataOptions referenceDataOptions = null,
            IJsonSerializationService jsonSerializationService = null)
        {
            return new DevolvedPostcodesRepositoryService(postcodesContextFactory, referenceDataOptions, jsonSerializationService);
        }

        private Mock<DevolvedPostcodesRepositoryService> NewServiceMock(
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IReferenceDataOptions referenceDataOptions = null,
            IJsonSerializationService jsonSerializationService = null)
        {
            return new Mock<DevolvedPostcodesRepositoryService>(postcodesContextFactory, referenceDataOptions, jsonSerializationService);
        }
    }
}
