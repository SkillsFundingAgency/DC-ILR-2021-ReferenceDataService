using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
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
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;

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
            var ilrReferenceDataRepositoryServiceMock = new Mock<IIlrReferenceDataRepositoryService>();

            IEnumerable<LargeEmployerSourceFile> empSourceFileList = new List<LargeEmployerSourceFile>
            {
                new LargeEmployerSourceFile { Id = 1 },
                new LargeEmployerSourceFile { Id = 2 },
            };

            IEnumerable<LarsVersion> larsList = new List<LarsVersion>
            {
                new LarsVersion { MainDataSchemaName = "Version0" },
                new LarsVersion { MainDataSchemaName = "Version1" },
            };

            IEnumerable<OrgVersion> orgList = new List<OrgVersion>
            {
                new OrgVersion { MainDataSchemaName = "Version0" },
                new OrgVersion { MainDataSchemaName = "Version1" },
                new OrgVersion { MainDataSchemaName = "Version3" },
            };

            IEnumerable<VersionInfo> postcoesList = new List<VersionInfo>
            {
                new VersionInfo { VersionNumber = "Version0" },
                new VersionInfo { VersionNumber = "Version1" },
                new VersionInfo { VersionNumber = "Version2" },
                new VersionInfo { VersionNumber = "Version3" },
                new VersionInfo { VersionNumber = "Version4" },
            };

            IReadOnlyCollection<ValidationError> validationErrors = new List<ValidationError>
            {
                new ValidationError { RuleName = "Rule1", Severity = SeverityLevel.Error, Message = "Message1" },
                new ValidationError { RuleName = "Rule2", Severity = SeverityLevel.Error, Message = "Message2" },
                new ValidationError { RuleName = "Rule3", Severity = SeverityLevel.Error, Message = "Message3" },
                new ValidationError { RuleName = "Rule4", Severity = SeverityLevel.Error, Message = "Message4" },
                new ValidationError { RuleName = "Rule5", Severity = SeverityLevel.Warning, Message = "Message5" },
                new ValidationError { RuleName = "Rule6", Severity = SeverityLevel.Warning, Message = "Message6" },
                new ValidationError { RuleName = "Rule7", Severity = SeverityLevel.Error, Message = "Message7" },
                new ValidationError { RuleName = "Rule8", Severity = SeverityLevel.Error, Message = "Message8" },
                new ValidationError { RuleName = "Rule9", Severity = SeverityLevel.Error, Message = "Message9" },
                new ValidationError { RuleName = "Rule10", Severity = SeverityLevel.Error, Message = "Message10" },
            };

            List<Lookup> lookups = new List<Lookup>
            {
                new Lookup { Code = "Lookup1", Name = "Lookup" },
                new Lookup { Code = "Lookup2", Name = "Lookup" },
                new Lookup { Code = "Lookup3", Name = "Lookup" },
            };

            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesList.AsQueryable().BuildMockDbSet();

            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveValidationErrorsAsync(CancellationToken.None)).Returns(Task.FromResult(validationErrors));
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveLookupsAsync(CancellationToken.None)).Returns(Task.FromResult(lookups));

            var serviceResult = await NewService(
                employersMock.Object,
                larsMock.Object,
                orgMock.Object,
                postcodesMock.Object,
                ilrReferenceDataRepositoryServiceMock.Object).RetrieveAsync(CancellationToken.None);

            serviceResult.ReferenceDataVersions.LarsVersion.Version.Should().BeEquivalentTo(larsVersion);
            serviceResult.ReferenceDataVersions.Employers.Version.Should().BeEquivalentTo(employersVersion);
            serviceResult.ReferenceDataVersions.OrganisationsVersion.Version.Should().BeEquivalentTo(orgVersion);
            serviceResult.ReferenceDataVersions.PostcodesVersion.Version.Should().BeEquivalentTo(postcodesVersion);
            serviceResult.ValidationErrors.Should().BeEquivalentTo(validationErrors);
        }

        [Fact]
        public async Task RetrieveAsync_ThrowsException()
        {
            var employersMock = new Mock<IEmployersContext>();
            var larsMock = new Mock<ILARSContext>();
            var orgMock = new Mock<IOrganisationsContext>();
            var postcodesMock = new Mock<IPostcodesContext>();
            var ilrReferenceDataRepositoryServiceMock = new Mock<IIlrReferenceDataRepositoryService>();

            IEnumerable<LargeEmployerSourceFile> empSourceFileList = new List<LargeEmployerSourceFile>
            {
                new LargeEmployerSourceFile { Id = 1 },
                new LargeEmployerSourceFile { Id = 2 },
            };

            IEnumerable<LarsVersion> larsList = new List<LarsVersion>();

            IEnumerable<OrgVersion> orgList = new List<OrgVersion>
            {
                new OrgVersion { MainDataSchemaName = "Version0" },
                new OrgVersion { MainDataSchemaName = "Version1" },
                new OrgVersion { MainDataSchemaName = "Version3" },
            };

            IEnumerable<VersionInfo> postcoesList = new List<VersionInfo>
            {
                new VersionInfo { VersionNumber = "Version0" },
                new VersionInfo { VersionNumber = "Version1" },
                new VersionInfo { VersionNumber = "Version2" },
                new VersionInfo { VersionNumber = "Version3" },
                new VersionInfo { VersionNumber = "Version4" },
            };

            IReadOnlyCollection<ValidationError> validationErrors = new List<ValidationError>
            {
                new ValidationError { RuleName = "Rule1", Severity = SeverityLevel.Error, Message = "Message1" },
                new ValidationError { RuleName = "Rule2", Severity = SeverityLevel.Error, Message = "Message2" },
                new ValidationError { RuleName = "Rule3", Severity = SeverityLevel.Error, Message = "Message3" },
                new ValidationError { RuleName = "Rule4", Severity = SeverityLevel.Error, Message = "Message4" },
                new ValidationError { RuleName = "Rule5", Severity = SeverityLevel.Warning, Message = "Message5" },
                new ValidationError { RuleName = "Rule6", Severity = SeverityLevel.Warning, Message = "Message6" },
                new ValidationError { RuleName = "Rule7", Severity = SeverityLevel.Error, Message = "Message7" },
                new ValidationError { RuleName = "Rule8", Severity = SeverityLevel.Error, Message = "Message8" },
                new ValidationError { RuleName = "Rule9", Severity = SeverityLevel.Error, Message = "Message9" },
                new ValidationError { RuleName = "Rule10", Severity = SeverityLevel.Error, Message = "Message10" },
            };

            List<Lookup> lookups = new List<Lookup>
            {
                new Lookup { Code = "Lookup1", Name = "Lookup" },
                new Lookup { Code = "Lookup2", Name = "Lookup" },
                new Lookup { Code = "Lookup3", Name = "Lookup" },
            };

            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesList.AsQueryable().BuildMockDbSet();

            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveValidationErrorsAsync(CancellationToken.None)).Returns(Task.FromResult(validationErrors));
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveLookupsAsync(CancellationToken.None)).Returns(Task.FromResult(lookups));

            Func<Task> serviceResult = async () =>
            {
                await NewService(
                         employersMock.Object,
                         larsMock.Object,
                         orgMock.Object,
                         postcodesMock.Object,
                         ilrReferenceDataRepositoryServiceMock.Object).RetrieveAsync(CancellationToken.None);
            };

            serviceResult.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values." +
                "Parameter name: MetaData Retrieval Error - Reference Dataset incomplete");
        }

        private MetaDataRetrievalService NewService(
            IEmployersContext employers = null,
            ILARSContext larsContext = null,
            IOrganisationsContext organisationsContext = null,
            IPostcodesContext postcodesContext = null,
            IIlrReferenceDataRepositoryService ilrReferenceDataRepositoryService = null)
        {
            return new MetaDataRetrievalService(employers, larsContext, organisationsContext, postcodesContext, ilrReferenceDataRepositoryService);
        }
    }
}
