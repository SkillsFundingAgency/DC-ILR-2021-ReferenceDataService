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
    public class LarsLearningDeliveryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var learnAimRefs = new List<string> { "LearnAimRef1", "LearnAimRef2", "LearnAimRef3" };

            var larsMock = new Mock<ILARSContext>();

            IEnumerable<LarsLearningDelivery> larsLearningDeliveryList = new List<LarsLearningDelivery>
            {
                new LarsLearningDelivery
                {
                    LearnAimRef = "LearnAimRef1",
                    LearnAimRefTitle = "AimRefTitle1",
                    LarsFrameworkAims = new List<LarsFrameworkAim>
                    {
                        new LarsFrameworkAim
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FworkCode = 1
                        },
                        new LarsFrameworkAim
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FworkCode = 2
                        }
                    },
                    LarsFundings = new List<LarsFunding>
                    {
                        new LarsFunding
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FundingCategory = "Cat1"
                        },
                        new LarsFunding
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FundingCategory = "Cat2"
                        },
                        new LarsFunding
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FundingCategory = "Cat3"
                        }
                    },
                    LarsValidities = new List<LarsValidity>
                    {
                        new LarsValidity
                        {
                            LearnAimRef = "LearnAimRef1",
                            StartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat1"
                        },
                        new LarsValidity
                        {
                            LearnAimRef = "LearnAimRef1",
                            StartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat2"
                        }
                    }
                },
                new LarsLearningDelivery
                {
                    LearnAimRef = "LearnAimRef2",
                    LearnAimRefTitle = "AimRefTitle2",
                    LarsLearningDeliveryCategories = new List<LarsLearningDeliveryCategory>
                    {
                        new LarsLearningDeliveryCategory
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            CategoryRef = 1
                        },
                        new LarsLearningDeliveryCategory
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            CategoryRef = 2
                        }
                    },
                    LarsAnnualValues = new List<LarsAnnualValue>
                    {
                        new LarsAnnualValue
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            BasicSkills = 1
                        }
                    },
                    LarsCareerLearningPilots = new List<LarsCareerLearningPilot>
                    {
                        new LarsCareerLearningPilot
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            AreaCode = "Area1"
                        },
                        new LarsCareerLearningPilot
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            AreaCode = "Area1"
                        }
                    }
                },
            };

            var larsLearningDeliveryMock = larsLearningDeliveryList.AsQueryable().BuildMockDbSet();

            larsMock.Setup(l => l.LARS_LearningDeliveries).Returns(larsLearningDeliveryMock.Object);

            var lars = await NewService(larsMock.Object).RetrieveAsync(learnAimRefs, CancellationToken.None);

            lars.Select(k => k.Key).Should().HaveCount(2);
            lars.Select(k => k.Key).Should().Contain("LearnAimRef1");
            lars.Select(k => k.Key).Should().Contain("LearnAimRef2");
            lars.Select(k => k.Key).Should().NotContain("LearnAimRef3");

            lars["LearnAimRef1"].LearnAimRefTitle.Should().Be("AimRefTitle1");
            lars["LearnAimRef1"].LARSFrameworkAims.Should().HaveCount(2);
            lars["LearnAimRef1"].LARSFundings.Should().HaveCount(3);
            lars["LearnAimRef1"].LARSValidities.Should().HaveCount(2);

            lars["LearnAimRef2"].LearnAimRefTitle.Should().Be("AimRefTitle2");
            lars["LearnAimRef2"].LARSLearningDeliveryCategories.Should().HaveCount(2);
            lars["LearnAimRef2"].LARSAnnualValues.Should().HaveCount(1);
            lars["LearnAimRef2"].LARSCareerLearningPilots.Should().HaveCount(2);
        }

        private LarsLearningDeliveryService NewService(ILARSContext larsContext = null)
        {
            return new LarsLearningDeliveryService(larsContext);
        }
    }
}
