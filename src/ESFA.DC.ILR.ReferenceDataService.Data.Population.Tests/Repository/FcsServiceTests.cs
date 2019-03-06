﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class FcsServiceTests
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
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
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
                            UnitCode = 1.0m
                        }
                    }
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "101",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 4),
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
                            UnitCode = 2.0m
                        }
                    }
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "102",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    DeliveryUkprn = ukprn
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "103",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 4),
                    DeliveryUkprn = ukprn
                },
                new ContractAllocation
                {
                    ContractAllocationNumber = "104",
                    FundingStreamCode = "Code1",
                    FundingStreamPeriodCode = "PeriodCode1",
                    Period = "R01",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2018, 8, 3),
                    DeliveryUkprn = 2
                }
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
                        }
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
                        }
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
                        }
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
                        }
                    }
                },
                new EsfEligibilityRule()
                {
                    LotReference = "Lot2",
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
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        },
                        new EsfEligibilityRuleEmploymentStatus
                        {
                            Code = 2,
                            LotReference = "Lot2",
                            TenderSpecReference = "TenderSpec2",
                        }
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
                        }
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
                        }
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
                        }
                    }
                }
            };

            var contractDeliverableCodeMapping = new List<ContractDeliverableCodeMapping>()
            {
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "PeriodCode1",
                    FcsdeliverableCode = "1",
                    ExternalDeliverableCode = "1"
                },
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "PeriodCode1",
                    FcsdeliverableCode = "2",
                    ExternalDeliverableCode = "2"
                }
            };

            var contractAllocationsMock = allocations.AsQueryable().BuildMockDbSet();
            var eligibilityRulesMock = eligibilityRules.AsQueryable().BuildMockDbSet();
            var contractDeliverableCodeMappingMock = contractDeliverableCodeMapping.AsQueryable().BuildMockDbSet();

            var fcsMock = new Mock<IFcsContext>();

            fcsMock.Setup(f => f.ContractAllocations).Returns(contractAllocationsMock.Object);
            fcsMock.Setup(f => f.EsfEligibilityRules).Returns(eligibilityRulesMock.Object);
            fcsMock.Setup(f => f.ContractDeliverableCodeMappings).Returns(contractDeliverableCodeMappingMock.Object);

            var service = await NewService(fcsMock.Object).RetrieveAsync(ukprn, CancellationToken.None);

            service.Should().HaveCount(4);
            service.Keys.Should().BeEquivalentTo(new List<string> { "100", "101", "102", "103" });

            service["100"].FCSContractDeliverables.Should().NotBeNullOrEmpty();
            service["101"].FCSContractDeliverables.Should().NotBeNullOrEmpty();
            service["102"].FCSContractDeliverables.Should().BeNullOrEmpty();

            service["100"].EsfEligibilityRule.Should().NotBeNull();
            service["100"].EsfEligibilityRule.TenderSpecReference.Should().Be("TenderSpec1");
            service["101"].EsfEligibilityRule.Should().NotBeNull();
            service["101"].EsfEligibilityRule.TenderSpecReference.Should().Be("TenderSpec2");
            service["102"].EsfEligibilityRule.Should().BeNull();
        }

        private FcsService NewService(IFcsContext fcs = null)
        {
            return new FcsService(fcs);
        }
    }
}