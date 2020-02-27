using System;
using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class MapperTests
    {
        [Fact]
        public void LarsLearningDeliveryReferenceDataMapper_Retrieve()
        {
            var learnAimRefs = new List<LARSLearningDeliveryKey>
            {
                new LARSLearningDeliveryKey("1", 1, 2, 3),
                new LARSLearningDeliveryKey("2", 2, 2, 3),
                new LARSLearningDeliveryKey("3", 1, 2, 4),
                new LARSLearningDeliveryKey("4", 1, 2, 3),
            };

            var expectedResult = new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery
                {
                    LearnAimRef = "1",
                    LARSFrameworks = new List<LARSFramework>
                    {
                        new LARSFramework
                        {
                            FworkCode = 1,
                            ProgType = 2,
                            PwayCode = 3,
                            LARSFrameworkAim = new LARSFrameworkAim
                            {
                                LearnAimRef = "1"
                            }
                        }
                    },
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LARSFrameworks = new List<LARSFramework>
                    {
                        new LARSFramework
                        {
                            FworkCode = 2,
                            ProgType = 2,
                            PwayCode = 3,
                        }
                    }
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "3",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
            };

            var referenceData = TestReferenceData();

            new LarsLearningDeliveryReferenceDataMapper().Map(learnAimRefs, referenceData).Should().BeEquivalentTo(expectedResult);
        }

        private DesktopReferenceDataRoot TestReferenceData()
        {
            return new DesktopReferenceDataRoot
            {
                LARSLearningDeliveries = new List<LARSLearningDelivery>
                {
                    new LARSLearningDelivery
                    {
                        LearnAimRef = "1",
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSLearningDelivery
                    {
                        LearnAimRef = "2",
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSLearningDelivery
                    {
                        LearnAimRef = "3",
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                },
                LARSFrameworks = new List<LARSFrameworkDesktop>
                {
                    new LARSFrameworkDesktop
                    {
                        FworkCode = 1,
                        ProgType = 2,
                        PwayCode = 3,
                    },
                    new LARSFrameworkDesktop
                    {
                        FworkCode = 2,
                        ProgType = 2,
                        PwayCode = 3,
                    },
                    new LARSFrameworkDesktop
                    {
                        FworkCode = 2,
                        ProgType = 2,
                        PwayCode = 2,
                    },
                },
                LARSStandards = new List<LARSStandard>
                {
                    new LARSStandard
                    {
                        StandardCode = 1,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSStandard
                    {
                        StandardCode = 2,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSStandard
                    {
                        StandardCode = 3,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSStandard
                    {
                        StandardCode = 33,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSStandard
                    {
                        StandardCode = 34,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                },
                LARSFrameworkAims = new List<LARSFrameworkAimDesktop>
                {
                    new LARSFrameworkAimDesktop
                    {
                        LearnAimRef = "1",
                        FworkCode = 1,
                        ProgType = 2,
                        PwayCode = 3,
                    },
                    new LARSFrameworkAimDesktop
                    {
                        LearnAimRef = "2",
                        FworkCode = 1,
                        ProgType = 2,
                        PwayCode = 3,
                    }
                }
            };
        }
    }
}
