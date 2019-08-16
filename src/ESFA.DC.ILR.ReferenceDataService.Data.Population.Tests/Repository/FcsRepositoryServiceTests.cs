using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class FcsRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var ukprn = 1;

            var allocations = new List<ContractAllocation>
            {
                new ContractAllocation
                {
                    ContractAllocationNumber = "100",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    LearningRatePremiumFactor = 1.0m,
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = 1,
                    LotReference = "Lot1",
                    TenderSpecReference = "TenderSpec1",
                    ContractDeliverables = new List<ContractDeliverable>
                    {
                        new ContractDeliverable
                        {
                            DeliverableCode = 1,
                            Description = "Description1",
                            PlannedValue = 1.0m,
                            PlannedVolume = 1,
                            UnitCost = 1.0m,
                            UnitCode = 1.0m,
                        },
                    },
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "101",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    LearningRatePremiumFactor = 2.0m,
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 4),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = ukprn,
                    LotReference = "Lot2",
                    TenderSpecReference = "TenderSpec2",
                    ContractDeliverables = new List<ContractDeliverable>
                    {
                        new ContractDeliverable
                        {
                            DeliverableCode = 2,
                            Description = "Description2",
                            PlannedValue = 2.0m,
                            PlannedVolume = 2,
                            UnitCost = 2.0m,
                            UnitCode = 2.0m,
                        },
                    },
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "102",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = ukprn,
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "103",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 4),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = ukprn,
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "104",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = 2,
                },
            };

            var eligibilityRules = new List<EsfEligibilityRule>()
            {
                new EsfEligibilityRule()
                {
                    LotReference = "Lot1",
                    TenderSpecReference = "TenderSpec1",
                    Benefits = true,
                    CalcMethod = 1,
                    MaxAge = 1,
                    MinAge = 1,
                    MinLengthOfUnemployment = 1,
                    MaxLengthOfUnemployment = 1,
                    MinPriorAttainment = "MinAttain",
                    MaxPriorAttainment = "MaxAttain",
                    EsfEligibilityRuleEmploymentStatuses = new List<EsfEligibilityRuleEmploymentStatus>
                    {
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 1,
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 2,
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                    },
                    EsfEligibilityRuleLocalAuthorities = new List<EsfEligibilityRuleLocalAuthority>
                    {
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "1",
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "2",
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                    },
                    EsfEligibilityRuleLocalEnterprisePartnerships = new List<EsfEligibilityRuleLocalEnterprisePartnership>
                    {
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "1",
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "2",
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                    },
                    EsfEligibilityRuleSectorSubjectAreaLevels = new List<EsfEligibilityRuleSectorSubjectAreaLevel>
                    {
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "1",
                            MaxLevelCode = "10",
                            SectorSubjectAreaCode = 1,
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "2",
                            MaxLevelCode = "20",
                            SectorSubjectAreaCode = 2,
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                    },
                },
                new EsfEligibilityRule()
                {
                    LotReference = "Lot2",
                    TenderSpecReference = "TenderSpec2",
                    Benefits = null,
                    CalcMethod = 1,
                    MaxAge = 1,
                    MinAge = 1,
                    MinLengthOfUnemployment = 1,
                    MaxLengthOfUnemployment = 1,
                    MinPriorAttainment = "MinAttain",
                    MaxPriorAttainment = "MaxAttain",
                    EsfEligibilityRuleEmploymentStatuses = new List<EsfEligibilityRuleEmploymentStatus>
                    {
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 1,
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 2,
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                    },
                    EsfEligibilityRuleLocalAuthorities = new List<EsfEligibilityRuleLocalAuthority>
                    {
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "1",
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "2",
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                    },
                    EsfEligibilityRuleLocalEnterprisePartnerships = new List<EsfEligibilityRuleLocalEnterprisePartnership>
                    {
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "1",
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "2",
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                    },
                    EsfEligibilityRuleSectorSubjectAreaLevels = new List<EsfEligibilityRuleSectorSubjectAreaLevel>
                    {
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "1",
                            MaxLevelCode = "10",
                            SectorSubjectAreaCode = 1,
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "2",
                            MaxLevelCode = "20",
                            SectorSubjectAreaCode = 2,
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                    },
                },
            };

            var contractDeliverableCodeMapping = new List<ContractDeliverableCodeMapping>()
            {
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "PeriodCode1",
                    FcsdeliverableCode = "1",
                    ExternalDeliverableCode = "1",
                },
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "PeriodCode1",
                    FcsdeliverableCode = "2",
                    ExternalDeliverableCode = "2",
                },
            };

            var contractAllocationsMock = allocations.AsQueryable().BuildMockDbSet();
            var eligibilityRulesMock = eligibilityRules.AsQueryable().BuildMockDbSet();
            var contractDeliverableCodeMappingMock = contractDeliverableCodeMapping.AsQueryable().BuildMockDbSet();

            var fcsMock = new Mock<IFcsContext>();

            fcsMock.Setup(f => f.ContractAllocations).Returns(contractAllocationsMock.Object);
            fcsMock.Setup(f => f.EsfEligibilityRules).Returns(eligibilityRulesMock.Object);
            fcsMock.Setup(f => f.ContractDeliverableCodeMappings).Returns(contractDeliverableCodeMappingMock.Object);

            var fcsContextFactoryMock = new Mock<IDbContextFactory<IFcsContext>>();
            fcsContextFactoryMock.Setup(c => c.Create()).Returns(fcsMock.Object);

            var service = await NewService(fcsContextFactoryMock.Object).RetrieveAsync(ukprn, CancellationToken.None);

            service.Should().HaveCount(4);
            service.Select(f => f.ContractAllocationNumber).Should().BeEquivalentTo(new List<string> { "100", "101", "102", "103" });

            service.Where(f => f.ContractAllocationNumber == "100").SelectMany(f => f.FCSContractDeliverables).Should().NotBeNullOrEmpty();
            service.Where(f => f.ContractAllocationNumber == "101").SelectMany(f => f.FCSContractDeliverables).Should().NotBeNullOrEmpty();
            service.Where(f => f.ContractAllocationNumber == "102").SelectMany(f => f.FCSContractDeliverables).Should().HaveCount(0);

            service.Where(f => f.ContractAllocationNumber == "100").SingleOrDefault().EsfEligibilityRule.Should().NotBeNull();
            service.Where(f => f.ContractAllocationNumber == "100").SingleOrDefault().EsfEligibilityRule.TenderSpecReference.Should().BeEquivalentTo("TenderSpec1");
            service.Where(f => f.ContractAllocationNumber == "101").SingleOrDefault().EsfEligibilityRule.Should().NotBeNull();
            service.Where(f => f.ContractAllocationNumber == "101").SingleOrDefault().EsfEligibilityRule.TenderSpecReference.Should().BeEquivalentTo("TenderSpec2");
            service.Where(f => f.ContractAllocationNumber == "102").SingleOrDefault().EsfEligibilityRule.Should().BeNull();

            service.Where(f => f.ContractAllocationNumber == "101").SingleOrDefault().EsfEligibilityRule.Benefits.Should().BeNull();
        }

        [Fact]
        public async Task RetrieveAsync_EsfEligibilityCaseSensitivity()
        {
            var ukprn = 1;

            var allocations = new List<ContractAllocation>
            {
                new ContractAllocation
                {
                    ContractAllocationNumber = "100",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    LearningRatePremiumFactor = 1.0m,
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = 1,
                    LotReference = "Lot1",
                    TenderSpecReference = "TenderSpec1",
                    ContractDeliverables = new List<ContractDeliverable>
                    {
                        new ContractDeliverable
                        {
                            DeliverableCode = 1,
                            Description = "Description1",
                            PlannedValue = 1.0m,
                            PlannedVolume = 1,
                            UnitCost = 1.0m,
                            UnitCode = 1.0m,
                        },
                    },
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "101",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    LearningRatePremiumFactor = 2.0m,
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 4),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = ukprn,
                    LotReference = "Lot2",
                    TenderSpecReference = "TenderSpec2",
                    ContractDeliverables = new List<ContractDeliverable>
                    {
                        new ContractDeliverable
                        {
                            DeliverableCode = 2,
                            Description = "Description2",
                            PlannedValue = 2.0m,
                            PlannedVolume = 2,
                            UnitCost = 2.0m,
                            UnitCode = 2.0m,
                        },
                    },
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "102",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = ukprn,
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "103",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 4),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = ukprn,
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "104",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    StopNewStartsFromDate = null,
                    DeliveryUkprn = 2,
                },
            };

            var eligibilityRules = new List<EsfEligibilityRule>()
            {
                new EsfEligibilityRule()
                {
                    LotReference = "Lot1",
                    TenderSpecReference = "tenderSpec1",
                    Benefits = true,
                    CalcMethod = 1,
                    MaxAge = 1,
                    MinAge = 1,
                    MinLengthOfUnemployment = 1,
                    MaxLengthOfUnemployment = 1,
                    MinPriorAttainment = "MinAttain",
                    MaxPriorAttainment = "MaxAttain",
                    EsfEligibilityRuleEmploymentStatuses = new List<EsfEligibilityRuleEmploymentStatus>
                    {
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 1,
                            LotReference = "lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 2,
                            LotReference = "LoT1",
                            TenderSpecReference = "TenderSpec1",
                        },
                    },
                    EsfEligibilityRuleLocalAuthorities = new List<EsfEligibilityRuleLocalAuthority>
                    {
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "1",
                            LotReference = "Lot1",
                            TenderSpecReference = "Tenderspec1",
                        },
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "2",
                            LotReference = "Lot1",
                            TenderSpecReference = "tenderSpec1",
                        },
                    },
                    EsfEligibilityRuleLocalEnterprisePartnerships = new List<EsfEligibilityRuleLocalEnterprisePartnership>
                    {
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "1",
                            LotReference = "lot1",
                            TenderSpecReference = "tenderspec1",
                        },
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "2",
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                    },
                    EsfEligibilityRuleSectorSubjectAreaLevels = new List<EsfEligibilityRuleSectorSubjectAreaLevel>
                    {
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "1",
                            MaxLevelCode = "10",
                            SectorSubjectAreaCode = 1,
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "2",
                            MaxLevelCode = "20",
                            SectorSubjectAreaCode = 2,
                            LotReference = "Lot1",
                            TenderSpecReference = "TenderSpec1",
                        },
                    },
                },
                new EsfEligibilityRule()
                {
                    LotReference = "lot2",
                    TenderSpecReference = "TenderSpec2",
                    Benefits = true,
                    CalcMethod = 1,
                    MaxAge = 1,
                    MinAge = 1,
                    MinLengthOfUnemployment = 1,
                    MaxLengthOfUnemployment = 1,
                    MinPriorAttainment = "MinAttain",
                    MaxPriorAttainment = "MaxAttain",
                    EsfEligibilityRuleEmploymentStatuses = new List<EsfEligibilityRuleEmploymentStatus>
                    {
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 1,
                            LotReference = "lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 2,
                            LotReference = "Lot2",
                            TenderSpecReference = "tenderSpec2",
                        },
                    },
                    EsfEligibilityRuleLocalAuthorities = new List<EsfEligibilityRuleLocalAuthority>
                    {
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "1",
                            LotReference = "Lot2",
                            TenderSpecReference = "tenderSpec2",
                        },
                        new EsfEligibilityRuleLocalAuthority
                        {
                            Code = "2",
                            LotReference = "lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                    },
                    EsfEligibilityRuleLocalEnterprisePartnerships = new List<EsfEligibilityRuleLocalEnterprisePartnership>
                    {
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "1",
                            LotReference = "lot2",
                            TenderSpecReference = "tenderSpec2",
                        },
                        new EsfEligibilityRuleLocalEnterprisePartnership
                        {
                            Code = "2",
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                    },
                    EsfEligibilityRuleSectorSubjectAreaLevels = new List<EsfEligibilityRuleSectorSubjectAreaLevel>
                    {
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "1",
                            MaxLevelCode = "10",
                            SectorSubjectAreaCode = 1,
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                        new EsfEligibilityRuleSectorSubjectAreaLevel
                        {
                            MinLevelCode = "2",
                            MaxLevelCode = "20",
                            SectorSubjectAreaCode = 2,
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                    },
                },
            };

            var contractDeliverableCodeMapping = new List<ContractDeliverableCodeMapping>()
            {
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "PeriodCode1",
                    FcsdeliverableCode = "1",
                    ExternalDeliverableCode = "1",
                },
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "PeriodCode1",
                    FcsdeliverableCode = "2",
                    ExternalDeliverableCode = "2",
                },
            };

            var contractAllocationsMock = allocations.AsQueryable().BuildMockDbSet();
            var eligibilityRulesMock = eligibilityRules.AsQueryable().BuildMockDbSet();
            var contractDeliverableCodeMappingMock = contractDeliverableCodeMapping.AsQueryable().BuildMockDbSet();

            var fcsMock = new Mock<IFcsContext>();

            fcsMock.Setup(f => f.ContractAllocations).Returns(contractAllocationsMock.Object);
            fcsMock.Setup(f => f.EsfEligibilityRules).Returns(eligibilityRulesMock.Object);
            fcsMock.Setup(f => f.ContractDeliverableCodeMappings).Returns(contractDeliverableCodeMappingMock.Object);

            var fcsContextFactoryMock = new Mock<IDbContextFactory<IFcsContext>>();
            fcsContextFactoryMock.Setup(c => c.Create()).Returns(fcsMock.Object);

            var service = await NewService(fcsContextFactoryMock.Object).RetrieveAsync(ukprn, CancellationToken.None);

            service.Should().HaveCount(4);
            service.Select(f => f.ContractAllocationNumber).Should().BeEquivalentTo(new List<string> { "100", "101", "102", "103" });

            service.Where(f => f.ContractAllocationNumber == "100").SelectMany(f => f.FCSContractDeliverables).Should().NotBeNullOrEmpty();
            service.Where(f => f.ContractAllocationNumber == "101").SelectMany(f => f.FCSContractDeliverables).Should().NotBeNullOrEmpty();
            service.Where(f => f.ContractAllocationNumber == "102").SelectMany(f => f.FCSContractDeliverables).Should().HaveCount(0);

            service.Where(f => f.ContractAllocationNumber == "100").SingleOrDefault().EsfEligibilityRule.Should().NotBeNull();
            service.Where(f => f.ContractAllocationNumber == "100").SingleOrDefault().EsfEligibilityRule.TenderSpecReference.Should().BeEquivalentTo("TenderSpec1");
            service.Where(f => f.ContractAllocationNumber == "101").SingleOrDefault().EsfEligibilityRule.Should().NotBeNull();
            service.Where(f => f.ContractAllocationNumber == "101").SingleOrDefault().EsfEligibilityRule.TenderSpecReference.Should().BeEquivalentTo("TenderSpec2");
            service.Where(f => f.ContractAllocationNumber == "102").SingleOrDefault().EsfEligibilityRule.Should().BeNull();
        }

        private FcsRepositoryService NewService(IDbContextFactory<IFcsContext> fcsContextFactory = null)
        {
            return new FcsRepositoryService(fcsContextFactory);
        }
    }
}