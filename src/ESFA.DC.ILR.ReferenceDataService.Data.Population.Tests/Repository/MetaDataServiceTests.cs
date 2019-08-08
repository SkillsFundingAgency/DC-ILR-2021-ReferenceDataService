﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.EAS1920.EF;
using ESFA.DC.EAS1920.EF.Interface;
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
            var utcDateTime = new DateTime(2019, 8, 1);
            var easDateTime = new DateTime(2019, 8, 1);

            var easMock = new Mock<IEasdbContext>();
            var employersMock = new Mock<IEmployersContext>();
            var larsMock = new Mock<ILARSContext>();
            var orgMock = new Mock<IOrganisationsContext>();
            var postcodesMock = new Mock<IPostcodesContext>();
            var ilrReferenceDataRepositoryServiceMock = new Mock<IIlrReferenceDataRepositoryService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            dateTimeProviderMock.Setup(dm => dm.GetNowUtc()).Returns(utcDateTime);

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

            var easDbMock = easSubmissionsList.AsQueryable().BuildMockDbSet();
            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesList.AsQueryable().BuildMockDbSet();

            easMock.Setup(e => e.EasSubmissions).Returns(easDbMock.Object);
            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveValidationErrorsAsync(CancellationToken.None)).Returns(Task.FromResult(validationErrors));
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveLookupsAsync(CancellationToken.None)).Returns(Task.FromResult(lookups));
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveValidationRulesAsync(CancellationToken.None)).Returns(Task.FromResult(validationRules));

            var serviceResult = await NewService(
                easMock.Object,
                employersMock.Object,
                larsMock.Object,
                orgMock.Object,
                postcodesMock.Object,
                ilrReferenceDataRepositoryServiceMock.Object,
                dateTimeProviderMock.Object).RetrieveAsync(1, CancellationToken.None);

            serviceResult.DateGenerated.Should().Be(utcDateTime);
            serviceResult.ReferenceDataVersions.LarsVersion.Version.Should().BeEquivalentTo(larsVersion);
            serviceResult.ReferenceDataVersions.Employers.Version.Should().BeEquivalentTo(employersVersion);
            serviceResult.ReferenceDataVersions.OrganisationsVersion.Version.Should().BeEquivalentTo(orgVersion);
            serviceResult.ReferenceDataVersions.PostcodesVersion.Version.Should().BeEquivalentTo(postcodesVersion);
            serviceResult.ReferenceDataVersions.EasUploadDateTime.UploadDateTime.Should().BeSameDateAs(easDateTime);
            serviceResult.ValidationErrors.Should().BeEquivalentTo(validationErrors);
            serviceResult.ValidationRules.Should().BeEquivalentTo(validationRules);
        }

        [Fact]
        public async Task RetrieveAsync_ThrowsException()
        {
            var easDateTime = new DateTime(2019, 8, 1);

            var easMock = new Mock<IEasdbContext>();
            var employersMock = new Mock<IEmployersContext>();
            var larsMock = new Mock<ILARSContext>();
            var orgMock = new Mock<IOrganisationsContext>();
            var postcodesMock = new Mock<IPostcodesContext>();
            var ilrReferenceDataRepositoryServiceMock = new Mock<IIlrReferenceDataRepositoryService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            dateTimeProviderMock.Setup(dm => dm.GetNowUtc()).Returns(DateTime.UtcNow);

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

            var easDbMock = easSubmissionsList.AsQueryable().BuildMockDbSet();
            var empDbMock = empSourceFileList.AsQueryable().BuildMockDbSet();
            var larsDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgDbMock = orgList.AsQueryable().BuildMockDbSet();
            var postcodesDbMock = postcoesList.AsQueryable().BuildMockDbSet();

            easMock.Setup(e => e.EasSubmissions).Returns(easDbMock.Object);
            employersMock.Setup(e => e.LargeEmployerSourceFiles).Returns(empDbMock.Object);
            larsMock.Setup(l => l.LARS_Versions).Returns(larsDbMock.Object);
            orgMock.Setup(o => o.OrgVersions).Returns(orgDbMock.Object);
            postcodesMock.Setup(p => p.VersionInfos).Returns(postcodesDbMock.Object);
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveValidationErrorsAsync(CancellationToken.None)).Returns(Task.FromResult(validationErrors));
            ilrReferenceDataRepositoryServiceMock.Setup(v => v.RetrieveLookupsAsync(CancellationToken.None)).Returns(Task.FromResult(lookups));

            Func<Task> serviceResult = async () =>
            {
                await NewService(
                         easMock.Object,
                         employersMock.Object,
                         larsMock.Object,
                         orgMock.Object,
                         postcodesMock.Object,
                         ilrReferenceDataRepositoryServiceMock.Object,
                         dateTimeProviderMock.Object).RetrieveAsync(1, CancellationToken.None);
            };

            serviceResult.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Specified argument was out of the range of valid values." +
                "Parameter name: MetaData Retrieval Error - Reference Dataset incomplete");
        }

        private MetaDataRetrievalService NewService(
            IEasdbContext eas = null,
            IEmployersContext employers = null,
            ILARSContext larsContext = null,
            IOrganisationsContext organisationsContext = null,
            IPostcodesContext postcodesContext = null,
            IIlrReferenceDataRepositoryService ilrReferenceDataRepositoryService = null,
            IDateTimeProvider dateTimeProvider = null)
        {
            return new MetaDataRetrievalService(
                eas,
                employers,
                larsContext,
                organisationsContext,
                postcodesContext,
                ilrReferenceDataRepositoryService,
                dateTimeProvider);
        }
    }
}
