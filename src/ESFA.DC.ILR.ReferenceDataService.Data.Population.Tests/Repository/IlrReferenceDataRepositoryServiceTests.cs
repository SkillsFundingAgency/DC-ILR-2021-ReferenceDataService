using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class IlrReferenceDataRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveValErrorsAsync()
        {
            var validationErrors = new List<ValidationError>
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

            var validationErrorsMock = new Mock<IIlrReferenceDataContext>();

            IEnumerable<Rule> errorsList = new List<Rule>
            {
                new Rule { Rulename = "Rule1", Severity = "E", Message = "Message1" },
                new Rule { Rulename = "Rule2", Severity = "E", Message = "Message2" },
                new Rule { Rulename = "Rule3", Severity = "E", Message = "Message3" },
                new Rule { Rulename = "Rule4", Severity = "E", Message = "Message4" },
                new Rule { Rulename = "Rule5", Severity = "W", Message = "Message5" },
                new Rule { Rulename = "Rule6", Severity = "W", Message = "Message6" },
                new Rule { Rulename = "Rule7", Severity = "E", Message = "Message7" },
                new Rule { Rulename = "Rule8", Severity = "E", Message = "Message8" },
                new Rule { Rulename = "Rule9", Severity = "E", Message = "Message9" },
                new Rule { Rulename = "Rule10", Severity = "E", Message = "Message10" },
            };

            var errorsListDbMock = errorsList.AsQueryable().BuildMockDbSet();

            validationErrorsMock.Setup(v => v.Rules).Returns(errorsListDbMock.Object);

            var serviceResult = await NewService(validationErrorsMock.Object).RetrieveValidationErrorsAsync(CancellationToken.None);

            serviceResult.Count().Should().Be(10);
            serviceResult.ToList().Should().BeEquivalentTo(validationErrors);
        }

        [Fact]
        public async Task RetrieveLookupsAsync()
        {
            var lookups = new List<ReferenceDataService.Model.MetaData.Lookup>
            {
                new ReferenceDataService.Model.MetaData.Lookup
                {
                    Name = "Lookup1",
                    Code = "Code1",
                    EffectiveFrom = new DateTime(1900, 1, 1),
                    EffectiveTo = new DateTime(2099, 1, 1),
                    SubCategories = new List<ReferenceDataService.Model.MetaData.LookupSubCategory>()
                },
                new ReferenceDataService.Model.MetaData.Lookup
                {
                    Name = "Lookup1",
                    Code = "Code2",
                    EffectiveFrom = new DateTime(1900, 1, 1),
                    EffectiveTo = new DateTime(2099, 1, 1),
                    SubCategories = new List<ReferenceDataService.Model.MetaData.LookupSubCategory>()
                },
                new ReferenceDataService.Model.MetaData.Lookup
                {
                    Name = "Lookup2",
                    Code = "Code1",
                    SubCategories = new List<ReferenceDataService.Model.MetaData.LookupSubCategory>()
                },
                new ReferenceDataService.Model.MetaData.Lookup
                {
                    Name = "Lookup3",
                    Code = "Code1",
                    EffectiveFrom = new DateTime(1900, 1, 1),
                    EffectiveTo = new DateTime(2099, 1, 1),
                    SubCategories = new List<ReferenceDataService.Model.MetaData.LookupSubCategory>
                    {
                        new ReferenceDataService.Model.MetaData.LookupSubCategory
                        {
                            Code = "Code3_1",
                            EffectiveFrom = new DateTime(1900, 1, 1),
                            EffectiveTo = new DateTime(2099, 1, 1),
                        },
                        new ReferenceDataService.Model.MetaData.LookupSubCategory
                        {
                            Code = "Code3_2",
                            EffectiveFrom = new DateTime(1900, 1, 1),
                            EffectiveTo = new DateTime(2099, 1, 1),
                        },
                    },
                },
            };

            var lookupsMock = new Mock<IIlrReferenceDataContext>();

            var lookupSubCategoriesList = new List<ILRReferenceData.Model.LookupSubCategory>
            {
                new ILRReferenceData.Model.LookupSubCategory
                {
                    ParentName = "Code3",
                    Name = "Code3SC",
                    Code = "Code3_1",
                    Description = "Description",
                    EffectiveFrom = new DateTime(1900, 1, 1),
                    EffectiveTo = new DateTime(2099, 1, 1),
                },
                new ILRReferenceData.Model.LookupSubCategory
                {
                    ParentName = "Code3",
                    Name = "Code3SC",
                    Code = "Code3_2",
                    Description = "Description",
                    EffectiveFrom = new DateTime(1900, 1, 1),
                    EffectiveTo = new DateTime(2099, 1, 1),
                },
            };

            IEnumerable<ILRReferenceData.Model.Lookup> lookupsList = new List<ILRReferenceData.Model.Lookup>
            {
                new ILRReferenceData.Model.Lookup { Name = "Lookup1", Description = "Description", Code = "Code1", EffectiveFrom = new DateTime(1900, 1, 1), EffectiveTo = new DateTime(2099, 1, 1) },
                new ILRReferenceData.Model.Lookup { Name = "Lookup1", Description = "Description", Code = "Code2", EffectiveFrom = new DateTime(1900, 1, 1), EffectiveTo = new DateTime(2099, 1, 1) },
                new ILRReferenceData.Model.Lookup { Name = "Lookup2", Description = "Description", Code = "Code1" },
                new ILRReferenceData.Model.Lookup
                {
                    Name = "Lookup3",
                    Description = "Description",
                    Code = "Code1",
                    EffectiveFrom = new DateTime(1900, 1, 1),
                    EffectiveTo = new DateTime(2099, 1, 1),
                    LookupSubCategories = lookupSubCategoriesList,
                },
            };

            var lookupListDbMock = lookupsList.AsQueryable().BuildMockDbSet();

            lookupsMock.Setup(v => v.Lookups).Returns(lookupListDbMock.Object);

            var serviceResult = await NewService(lookupsMock.Object).RetrieveLookupsAsync(CancellationToken.None);

            serviceResult.Count().Should().Be(4);
            serviceResult.ToList().Should().BeEquivalentTo(lookups);
        }

        [Fact]
        public async Task RetrieveCollectionDates()
        {
            var collectionDates = new IlrCollectionDates
            {
                CensusDates = new List<CensusDate>
                {
                    new CensusDate { Period = 01, Start = new DateTime(2019, 08, 01) },
                    new CensusDate { Period = 02, Start = new DateTime(2019, 09, 01) },
                    new CensusDate { Period = 03, Start = new DateTime(2019, 10, 01) },
                    new CensusDate { Period = 04, Start = new DateTime(2019, 11, 01) },
                    new CensusDate { Period = 05, Start = new DateTime(2019, 12, 01) },
                    new CensusDate { Period = 06, Start = new DateTime(2020, 01, 01) },
                    new CensusDate { Period = 07, Start = new DateTime(2020, 02, 01) },
                    new CensusDate { Period = 08, Start = new DateTime(2020, 03, 01) },
                    new CensusDate { Period = 09, Start = new DateTime(2020, 04, 01) },
                    new CensusDate { Period = 10, Start = new DateTime(2020, 05, 01) },
                    new CensusDate { Period = 11, Start = new DateTime(2020, 06, 01) },
                    new CensusDate { Period = 12, Start = new DateTime(2020, 07, 01) },
                },
                ReturnPeriods = new List<ReturnPeriod>
                {
                    new ReturnPeriod { Name = "R01", Period = 1, Start = new DateTime(2019, 08, 23), End = new DateTime(2019, 09, 06, 23, 59, 59) },
                    new ReturnPeriod { Name = "R02", Period = 2, Start = new DateTime(2019, 09, 07), End = new DateTime(2019, 10, 04, 23, 59, 59) },
                    new ReturnPeriod { Name = "R03", Period = 3, Start = new DateTime(2019, 10, 05), End = new DateTime(2019, 11, 06, 23, 59, 59) },
                    new ReturnPeriod { Name = "R04", Period = 4, Start = new DateTime(2019, 11, 07), End = new DateTime(2019, 12, 06, 23, 59, 59) },
                    new ReturnPeriod { Name = "R05", Period = 5, Start = new DateTime(2019, 12, 07), End = new DateTime(2020, 01, 07, 23, 59, 59) },
                    new ReturnPeriod { Name = "R06", Period = 6, Start = new DateTime(2020, 01, 08), End = new DateTime(2020, 02, 06, 23, 59, 59) },
                    new ReturnPeriod { Name = "R07", Period = 7, Start = new DateTime(2020, 02, 07), End = new DateTime(2020, 03, 06, 23, 59, 59) },
                    new ReturnPeriod { Name = "R08", Period = 8, Start = new DateTime(2020, 03, 07), End = new DateTime(2020, 04, 04, 23, 59, 59) },
                    new ReturnPeriod { Name = "R09", Period = 9, Start = new DateTime(2020, 04, 05), End = new DateTime(2020, 05, 07, 23, 59, 59) },
                    new ReturnPeriod { Name = "R10", Period = 10, Start = new DateTime(2020, 05, 08), End = new DateTime(2020, 06, 06, 23, 59, 59) },
                    new ReturnPeriod { Name = "R11", Period = 11, Start = new DateTime(2020, 06, 07), End = new DateTime(2020, 07, 04, 23, 59, 59) },
                    new ReturnPeriod { Name = "R12", Period = 12, Start = new DateTime(2020, 07, 05), End = new DateTime(2020, 08, 06, 23, 59, 59) },
                    new ReturnPeriod { Name = "R13", Period = 13, Start = new DateTime(2020, 08, 07), End = new DateTime(2020, 09, 13, 23, 59, 59) },
                    new ReturnPeriod { Name = "R14", Period = 14, Start = new DateTime(2020, 09, 14), End = new DateTime(2020, 10, 17, 23, 59, 59) },
                }
            };

            NewService().RetrieveCollectionDates().Should().BeEquivalentTo(collectionDates);
        }

        private IlrReferenceDataRepositoryService NewService(IIlrReferenceDataContext ilrReferenceDataContext = null)
        {
            return new IlrReferenceDataRepositoryService(ilrReferenceDataContext);
        }
    }
}
