using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class LarsStandardServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var stdCodes = new List<int> { 1, 2, 3 };

            var larsMock = new Mock<ILARSContext>();

            IEnumerable<LarsStandard> larsStandardList = new List<LarsStandard>
            {
                new LarsStandard
                {
                    StandardCode = 1,
                    StandardSectorCode = "SSC1",
                    NotionalEndLevel = "NEL1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LarsApprenticeshipStdFundings = new List<LarsApprenticeshipStdFunding>
                    {
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 1,
                            CareLeaverAdditionalPayment = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 2,
                            CareLeaverAdditionalPayment = 2.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardFundings = new List<LarsStandardFunding>
                    {
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 2.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        }
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat1",
                            StartDate = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat2",
                            StartDate = new DateTime(2018, 8, 1),
                        },
                    },
                },
                new LarsStandard
                {
                    StandardCode = 2,
                    StandardSectorCode = "SSC2",
                    NotionalEndLevel = "NEL2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LarsApprenticeshipStdFundings = new List<LarsApprenticeshipStdFunding>
                    {
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 3,
                            CareLeaverAdditionalPayment = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 4,
                            CareLeaverAdditionalPayment = 2.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 3,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 4,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardFundings = new List<LarsStandardFunding>
                    {
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 3.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 4.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        }
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat3",
                            StartDate = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat4"
                        }
                    }
                }
            };

            var larsStandardMock = larsStandardList.AsQueryable().BuildMockDbSet();

            larsMock.Setup(l => l.LARS_Standards).Returns(larsStandardMock.Object);

            var lars = await NewService(larsMock.Object).RetrieveAsync(stdCodes, CancellationToken.None);

            lars.Select(k => k.Key).Should().HaveCount(2);
            lars.Select(k => k.Key).Should().Contain(1);
            lars.Select(k => k.Key).Should().Contain(2);
            lars.Select(k => k.Key).Should().NotContain(3);

            lars[1].StandardSectorCode.Should().Be("SSC1");
            lars[1].LARSStandardApprenticeshipFundings.Should().HaveCount(2);
            lars[1].LARSStandardCommonComponents.Should().HaveCount(2);
            lars[1].LARSStandardFundings.Should().HaveCount(2);
            lars[1].LARSStandardValidities.Should().HaveCount(2);

            lars[2].StandardSectorCode.Should().Be("SSC2");
            lars[2].LARSStandardApprenticeshipFundings.Should().HaveCount(2);
            lars[2].LARSStandardCommonComponents.Should().HaveCount(2);
            lars[2].LARSStandardFundings.Should().HaveCount(2);
            lars[2].LARSStandardValidities.Should().HaveCount(2);
        }

        private LarsStandardService NewService(ILARSContext larsContext = null)
        {
            return new LarsStandardService(larsContext);
        }
    }
}
