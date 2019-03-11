using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.Employers.Model;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class MetaDataServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var larsVersion = "Version1";
            var employersVersion = "2";
            var orgVersion = "Version3";
            var postcodesVersion = "Version4";

            var employersMock = new Mock<IEmployersContext>();
            var larsMock = new Mock<ILARSContext>();
            var orgMock = new Mock<IOrganisationsContext>();
            var postcodesMock = new Mock<IPostcodesContext>();

            IEnumerable<LargeEmployerSourceFile> empSourceFileList = new List<LargeEmployerSourceFile>
            {
                new LargeEmployerSourceFile { Id = 1 },
                new LargeEmployerSourceFile { Id = 2 }
            };

            IEnumerable<LarsVersion> larsList = new List<LarsVersion>
            {
                new LarsVersion { MainDataSchemaName = "Version0" },
                new LarsVersion { MainDataSchemaName = "Version1" }
            };

            IEnumerable<OrgVersion> orgList = new List<OrgVersion>
            {
                new OrgVersion { MainDataSchemaName = "Version0" },
                new OrgVersion { MainDataSchemaName = "Version1" },
                new OrgVersion { MainDataSchemaName = "Version3" }
            };

            IEnumerable<VersionInfo> postcoesList = new List<VersionInfo>
            {
                new VersionInfo { VersionNumber = "Version0" },
                new VersionInfo { VersionNumber = "Version1" },
                new VersionInfo { VersionNumber = "Version2" },
                new VersionInfo { VersionNumber = "Version3" },
                new VersionInfo { VersionNumber = "Version4" }
            };

            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesList.AsQueryable().BuildMockDbSet();

            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);

            var serviceResult = await NewService(employersMock.Object, larsMock.Object, orgMock.Object, postcodesMock.Object).RetrieveAsync(CancellationToken.None);

            serviceResult.ReferenceDataVersions.Should().HaveCount(4);
            serviceResult.ReferenceDataVersions.Where(r => r.Name == "LARS Version").Select(r => r.Version).Should().BeEquivalentTo(larsVersion);
            serviceResult.ReferenceDataVersions.Where(r => r.Name == "Employers Version").Select(r => r.Version).Should().BeEquivalentTo(employersVersion);
            serviceResult.ReferenceDataVersions.Where(r => r.Name == "Organisations Version").Select(r => r.Version).Should().BeEquivalentTo(orgVersion);
            serviceResult.ReferenceDataVersions.Where(r => r.Name == "Potcodes Version").Select(r => r.Version).Should().BeEquivalentTo(postcodesVersion);
        }

        private MetaDataService NewService(
            IEmployersContext employers = null,
            ILARSContext larsContext = null,
            IOrganisationsContext organisationsContext = null,
            IPostcodesContext postcodesContext = null)
        {
            return new MetaDataService(employers, larsContext, organisationsContext, postcodesContext);
        }
    }
}
