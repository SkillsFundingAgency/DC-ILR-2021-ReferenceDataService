using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.EAS2021.EF;
using ESFA.DC.EAS2021.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
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
using ValidationError = ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;
using Version = ESFA.DC.ReferenceData.Organisations.Model.Version;

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
            var cofVersion = "1.1.1.1";
            var campusIdVersion = "2.2.2.2";
            var postcodesVersion = "Version4";
            var devolvedPostcodesVersion = "Version5";
            var hmppPostcodesVersion = "Version6";
            var postcodeFactorsVersion = "Version7";
            var utcDateTime = new DateTime(2019, 8, 1);
            var easDateTime = new DateTime(2019, 8, 2);
            var easFilename = "Eas2.csv";

            IEnumerable<SourceFile> easSourceFileList = new List<SourceFile>
            {
                new SourceFile { SourceFileId = 1,  Ukprn = "1", DateTime = new DateTime(2019, 8, 1), FileName = "1/Eas1.csv" },
                new SourceFile { SourceFileId = 2, Ukprn = "1", DateTime = new DateTime(2019, 8, 2), FileName = "1/Eas2.csv" },
                new SourceFile { SourceFileId = 3, Ukprn = "2", DateTime = new DateTime(2019, 9, 1), FileName = "2/Eas1.csv" }
            };

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

            IEnumerable<Version> orgVersions = new List<Version>
            {
                new Version { Source = "ConditionOfFunding", VersionNumber = cofVersion },
                new Version { Source = "CampusIdentifier", VersionNumber = campusIdVersion },
                new Version { Source = "Org", VersionNumber = orgVersion }
            };

            IEnumerable<VersionInfo> postcoesVersions = new List<VersionInfo>
            {
                new VersionInfo { DataSource = "OnsPostcodes", VersionNumber = "Version4" },
                new VersionInfo { DataSource = "DevolvedPostcodes", VersionNumber = "Version5" },
                new VersionInfo { DataSource = "HmppPostcode", VersionNumber = "Version6" },
                new VersionInfo { DataSource = "PostcodeFactors", VersionNumber = "Version7" }
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

            IReadOnlyCollection<ValidationRule> validationRules = new List<ValidationRule>
            {
                new ValidationRule { RuleName = "Rule1", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule2", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule3", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule4", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule5", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule6", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule7", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule8", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule9", Desktop = true, Online = true },
                new ValidationRule { RuleName = "Rule10", Desktop = true, Online = true },
            };

            List<Lookup> lookups = new List<Lookup>
            {
                new Lookup { Code = "Lookup1", Name = "Lookup" },
                new Lookup { Code = "Lookup2", Name = "Lookup" },
                new Lookup { Code = "Lookup3", Name = "Lookup" },
            };

            var metaData = new MetaData
            {
                Lookups = lookups,
                ValidationErrors = validationErrors,
                ValidationRules = validationRules
            };

            var easDbMock = easSourceFileList.AsQueryable().BuildMockDbSet();
            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var versionsDbMock = orgVersions.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesVersions.AsQueryable().BuildMockDbSet();

            var easMock = new Mock<IEasdbContext>();
            var employersMock = new Mock<IEmployersContext>();
            var larsMock = new Mock<ILARSContext>();
            var orgMock = new Mock<IOrganisationsContext>();
            var postcodesMock = new Mock<IPostcodesContext>();
            var ilrReferenceDataRepositoryServiceMock = new Mock<IIlrReferenceDataRepositoryService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var easContextFactoryMock = new Mock<IDbContextFactory<IEasdbContext>>();
            var employersContextFactoryMock = new Mock<IDbContextFactory<IEmployersContext>>();
            var larsContextFactoryMock = new Mock<IDbContextFactory<ILARSContext>>();
            var orgContextFactoryMock = new Mock<IDbContextFactory<IOrganisationsContext>>();
            var postcodesContextFactoryMock = new Mock<IDbContextFactory<IPostcodesContext>>();
            var ilrReferenceDataRepositoryServiceContextFactoryMock = new Mock<IDbContextFactory<IIlrReferenceDataRepositoryService>>();

            dateTimeProviderMock.Setup(dm => dm.GetNowUtc()).Returns(utcDateTime);

            easMock.Setup(e => e.SourceFiles).Returns(easDbMock.Object);
            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            orgMock.Setup(o => o.Versions).Returns(versionsDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);

            easContextFactoryMock.Setup(c => c.Create()).Returns(easMock.Object);
            employersContextFactoryMock.Setup(c => c.Create()).Returns(employersMock.Object);
            larsContextFactoryMock.Setup(c => c.Create()).Returns(larsMock.Object);
            orgContextFactoryMock.Setup(c => c.Create()).Returns(orgMock.Object);
            postcodesContextFactoryMock.Setup(c => c.Create()).Returns(postcodesMock.Object);
            ilrReferenceDataRepositoryServiceContextFactoryMock.Setup(c => c.Create()).Returns(ilrReferenceDataRepositoryServiceMock.Object);

            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveIlrReferenceDataAsync(CancellationToken.None)).Returns(Task.FromResult(metaData));

            var serviceResult = await NewService(
                easContextFactoryMock.Object,
                employersContextFactoryMock.Object,
                larsContextFactoryMock.Object,
                orgContextFactoryMock.Object,
                postcodesContextFactoryMock.Object,
                ilrReferenceDataRepositoryServiceMock.Object,
                dateTimeProviderMock.Object).RetrieveAsync(1, CancellationToken.None);

            serviceResult.DateGenerated.Should().Be(utcDateTime);
            serviceResult.ReferenceDataVersions.LarsVersion.Version.Should().BeEquivalentTo(larsVersion);
            serviceResult.ReferenceDataVersions.CoFVersion.Version.Should().Be(cofVersion);
            serviceResult.ReferenceDataVersions.CampusIdentifierVersion.Version.Should().Be(campusIdVersion);
            serviceResult.ReferenceDataVersions.Employers.Version.Should().BeEquivalentTo(employersVersion);
            serviceResult.ReferenceDataVersions.OrganisationsVersion.Version.Should().BeEquivalentTo(orgVersion);
            serviceResult.ReferenceDataVersions.PostcodesVersion.Version.Should().BeEquivalentTo(postcodesVersion);
            serviceResult.ReferenceDataVersions.DevolvedPostcodesVersion.Version.Should().BeEquivalentTo(devolvedPostcodesVersion);
            serviceResult.ReferenceDataVersions.HmppPostcodesVersion.Version.Should().BeEquivalentTo(hmppPostcodesVersion);
            serviceResult.ReferenceDataVersions.PostcodeFactorsVersion.Version.Should().BeEquivalentTo(postcodeFactorsVersion);
            serviceResult.ReferenceDataVersions.EasFileDetails.FileName.Should().BeEquivalentTo(easFilename);
            serviceResult.ReferenceDataVersions.EasFileDetails.UploadDateTime.Should().BeSameDateAs(easDateTime);
            serviceResult.ValidationErrors.Should().BeEquivalentTo(validationErrors);
            serviceResult.ValidationRules.Should().BeEquivalentTo(validationRules);
        }

        [Fact]
        public async Task RetrieveAsync_ThrowsExceptionForMultipleSources()
        {
            var easDateTime = new DateTime(2019, 8, 1);
            var orgVersion = "Version3";
            var cofVersion = "1.1.1.1";

            IEnumerable<EasSubmission> easSubmissionsList = new List<EasSubmission>
            {
                new EasSubmission { Ukprn = "1", UpdatedOn = new DateTime(2019, 8, 1) },
                new EasSubmission { Ukprn = "2", UpdatedOn = new DateTime(2019, 9, 1) }
            };

            IEnumerable<LargeEmployerSourceFile> empSourceFileList = new List<LargeEmployerSourceFile>
            {
                new LargeEmployerSourceFile { Id = 1 },
                new LargeEmployerSourceFile { Id = 2 },
            };

            IEnumerable<LarsVersion> larsList = new List<LarsVersion>
            {
                new LarsVersion { MainDataSchemaName = "Version0" },
                new LarsVersion { MainDataSchemaName = "Version1" },
                new LarsVersion { MainDataSchemaName = "Version2" },
            };

            IEnumerable<OrgVersion> orgList = new List<OrgVersion>
            {
                new OrgVersion { MainDataSchemaName = "Version0" },
                new OrgVersion { MainDataSchemaName = "Version1" },
                new OrgVersion { MainDataSchemaName = "Version3" },
            };

            IEnumerable<Version> versions = new List<Version>
            {
                new Version { Source = "ConditionOfFunding", VersionNumber = cofVersion },
                new Version { Source = "Org", VersionNumber = orgVersion }
            };

            IEnumerable<VersionInfo> postcoesVersions = new List<VersionInfo>
            {
                new VersionInfo { DataSource = "OnsPostcodes", VersionNumber = "Version4" },
                new VersionInfo { DataSource = "DevolvedPostcodes", VersionNumber = "Version5" },
                new VersionInfo { DataSource = "HmppPostcode", VersionNumber = "Version6" },
                new VersionInfo { DataSource = "PostcodeFactors", VersionNumber = "Version7" }
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

            var metaData = new MetaData
            {
                Lookups = lookups,
                ValidationErrors = validationErrors,
                ValidationRules = new List<ValidationRule>()
            };

            var easDbMock = easSubmissionsList.AsQueryable().BuildMockDbSet();
            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var versionsDbMock = versions.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesVersions.AsQueryable().BuildMockDbSet();

            var easMock = new Mock<IEasdbContext>();
            var employersMock = new Mock<IEmployersContext>();
            var larsMock = new Mock<ILARSContext>();
            var orgMock = new Mock<IOrganisationsContext>();
            var postcodesMock = new Mock<IPostcodesContext>();
            var ilrReferenceDataRepositoryServiceMock = new Mock<IIlrReferenceDataRepositoryService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var easContextFactoryMock = new Mock<IDbContextFactory<IEasdbContext>>();
            var employersContextFactoryMock = new Mock<IDbContextFactory<IEmployersContext>>();
            var larsContextFactoryMock = new Mock<IDbContextFactory<ILARSContext>>();
            var orgContextFactoryMock = new Mock<IDbContextFactory<IOrganisationsContext>>();
            var postcodesContextFactoryMock = new Mock<IDbContextFactory<IPostcodesContext>>();
            var ilrReferenceDataRepositoryServiceContextFactoryMock = new Mock<IDbContextFactory<IIlrReferenceDataRepositoryService>>();

            dateTimeProviderMock.Setup(dm => dm.GetNowUtc()).Returns(DateTime.UtcNow);

            easMock.Setup(e => e.EasSubmissions).Returns(easDbMock.Object);
            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            orgMock.Setup(o => o.Versions).Returns(versionsDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);

            easContextFactoryMock.Setup(c => c.Create()).Returns(easMock.Object);
            employersContextFactoryMock.Setup(c => c.Create()).Returns(employersMock.Object);
            larsContextFactoryMock.Setup(c => c.Create()).Returns(larsMock.Object);
            orgContextFactoryMock.Setup(c => c.Create()).Returns(orgMock.Object);
            postcodesContextFactoryMock.Setup(c => c.Create()).Returns(postcodesMock.Object);
            ilrReferenceDataRepositoryServiceContextFactoryMock.Setup(c => c.Create()).Returns(ilrReferenceDataRepositoryServiceMock.Object);

            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveIlrReferenceDataAsync(CancellationToken.None)).Returns(Task.FromResult(metaData));

            Func<Task> serviceResult = async () =>
            {
                await NewService(
                         easContextFactoryMock.Object,
                         employersContextFactoryMock.Object,
                         larsContextFactoryMock.Object,
                         orgContextFactoryMock.Object,
                         postcodesContextFactoryMock.Object,
                         ilrReferenceDataRepositoryServiceMock.Object,
                         dateTimeProviderMock.Object).RetrieveAsync(1, CancellationToken.None);
            };

            serviceResult.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values." +
                "Parameter name: MetaData Retrieval Error - Reference Dataset incomplete. Cannot find version information for dataset specified. The dataset may be incomplete. Please check the following data sources: Campus Identifiers, Validation Rules");
        }

        [Fact]
        public async Task RetrieveAsync_ThrowsExceptionForSingleSource()
        {
            var easDateTime = new DateTime(2019, 8, 1);
            var orgVersion = "Version3";
            var cofVersion = "1.1.1.1";
            var campusIdVersion = "2.2.2.2";

            IEnumerable<EasSubmission> easSubmissionsList = new List<EasSubmission>
            {
                new EasSubmission { Ukprn = "1", UpdatedOn = new DateTime(2019, 8, 1) },
                new EasSubmission { Ukprn = "2", UpdatedOn = new DateTime(2019, 9, 1) }
            };

            IEnumerable<LargeEmployerSourceFile> empSourceFileList = new List<LargeEmployerSourceFile>
            {
                new LargeEmployerSourceFile { Id = 1 },
                new LargeEmployerSourceFile { Id = 2 },
            };

            IEnumerable<LarsVersion> larsList = new List<LarsVersion>
            {
                new LarsVersion { MainDataSchemaName = "Version0" },
                new LarsVersion { MainDataSchemaName = "Version1" },
                new LarsVersion { MainDataSchemaName = "Version2" },
            };

            IEnumerable<OrgVersion> orgList = new List<OrgVersion>
            {
                new OrgVersion { MainDataSchemaName = "Version0" },
                new OrgVersion { MainDataSchemaName = "Version1" },
                new OrgVersion { MainDataSchemaName = "Version3" },
            };

            IEnumerable<Version> versions = new List<Version>
            {
                new Version { Source = "ConditionOfFunding", VersionNumber = cofVersion },
                new Version { Source = "CampusIdentifier", VersionNumber = campusIdVersion },
                new Version { Source = "Org", VersionNumber = orgVersion }
            };

            IEnumerable<VersionInfo> postcoesVersions = new List<VersionInfo>
            {
                new VersionInfo { DataSource = "OnsPostcodes", VersionNumber = "Version4" },
                new VersionInfo { DataSource = "DevolvedPostcodes", VersionNumber = "Version5" },
                new VersionInfo { DataSource = "HmppPostcode", VersionNumber = "Version6" },
                new VersionInfo { DataSource = "PostcodeFactors", VersionNumber = "Version7" }
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

            var metaData = new MetaData
            {
                Lookups = lookups,
                ValidationErrors = validationErrors,
                ValidationRules = new List<ValidationRule>()
            };

            var easDbMock = easSubmissionsList.AsQueryable().BuildMockDbSet();
            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var versionsDbMock = versions.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesVersions.AsQueryable().BuildMockDbSet();

            var easMock = new Mock<IEasdbContext>();
            var employersMock = new Mock<IEmployersContext>();
            var larsMock = new Mock<ILARSContext>();
            var orgMock = new Mock<IOrganisationsContext>();
            var postcodesMock = new Mock<IPostcodesContext>();
            var ilrReferenceDataRepositoryServiceMock = new Mock<IIlrReferenceDataRepositoryService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var easContextFactoryMock = new Mock<IDbContextFactory<IEasdbContext>>();
            var employersContextFactoryMock = new Mock<IDbContextFactory<IEmployersContext>>();
            var larsContextFactoryMock = new Mock<IDbContextFactory<ILARSContext>>();
            var orgContextFactoryMock = new Mock<IDbContextFactory<IOrganisationsContext>>();
            var postcodesContextFactoryMock = new Mock<IDbContextFactory<IPostcodesContext>>();
            var ilrReferenceDataRepositoryServiceContextFactoryMock = new Mock<IDbContextFactory<IIlrReferenceDataRepositoryService>>();

            dateTimeProviderMock.Setup(dm => dm.GetNowUtc()).Returns(DateTime.UtcNow);

            easMock.Setup(e => e.EasSubmissions).Returns(easDbMock.Object);
            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            orgMock.Setup(o => o.Versions).Returns(versionsDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);

            easContextFactoryMock.Setup(c => c.Create()).Returns(easMock.Object);
            employersContextFactoryMock.Setup(c => c.Create()).Returns(employersMock.Object);
            larsContextFactoryMock.Setup(c => c.Create()).Returns(larsMock.Object);
            orgContextFactoryMock.Setup(c => c.Create()).Returns(orgMock.Object);
            postcodesContextFactoryMock.Setup(c => c.Create()).Returns(postcodesMock.Object);
            ilrReferenceDataRepositoryServiceContextFactoryMock.Setup(c => c.Create()).Returns(ilrReferenceDataRepositoryServiceMock.Object);

            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveIlrReferenceDataAsync(CancellationToken.None)).Returns(Task.FromResult(metaData));

            Func<Task> serviceResult = async () =>
            {
                await NewService(
                         easContextFactoryMock.Object,
                         employersContextFactoryMock.Object,
                         larsContextFactoryMock.Object,
                         orgContextFactoryMock.Object,
                         postcodesContextFactoryMock.Object,
                         ilrReferenceDataRepositoryServiceMock.Object,
                         dateTimeProviderMock.Object).RetrieveAsync(1, CancellationToken.None);
            };

            serviceResult.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values." +
                "Parameter name: MetaData Retrieval Error - Reference Dataset incomplete. Cannot find version information for dataset specified. The dataset may be incomplete. Please check the following data sources: Validation Rules");
        }

        private MetaDataRetrievalService NewService(
            IDbContextFactory<IEasdbContext> easContextFactory = null,
            IDbContextFactory<IEmployersContext> employersContextFactory = null,
            IDbContextFactory<ILARSContext> larsContextFactory = null,
            IDbContextFactory<IOrganisationsContext> organisationsContextFactory = null,
            IDbContextFactory<IPostcodesContext> postcodesContextFactory = null,
            IIlrReferenceDataRepositoryService ilReferenceDataRepositoryService = null,
            IDateTimeProvider dateTimeProvider = null)
        {
            return new MetaDataRetrievalService(
                easContextFactory,
                employersContextFactory,
                larsContextFactory,
                organisationsContextFactory,
                postcodesContextFactory,
                ilReferenceDataRepositoryService,
                dateTimeProvider);
        }
    }
}
