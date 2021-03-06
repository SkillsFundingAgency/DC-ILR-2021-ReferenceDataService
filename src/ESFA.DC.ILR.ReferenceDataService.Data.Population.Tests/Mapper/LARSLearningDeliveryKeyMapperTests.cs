﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class LARSLearningDeliveryKeyMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var expectedLARSLearningDeliveryKeys = new List<LARSLearningDeliveryKey>
            {
                new LARSLearningDeliveryKey("LEARNAIMREF1", 1, 2, 3),
                new LARSLearningDeliveryKey("LEARNAIMREF2", 1, 2, 4),
                new LARSLearningDeliveryKey("LEARNAIMREF3", 1, null, 3),
            };

            var message = TestMessage();

            var mapper = NewMapper().MapLARSLearningDeliveryKeysFromMessage(message);

            mapper.Count().Should().Be(3);
            mapper.Should().BeEquivalentTo(expectedLARSLearningDeliveryKeys);
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapLARSLearningDeliveryKeysFromMessage(null).Should().BeNullOrEmpty();
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                LearnAimRef = "LEARNAIMREF1",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 3,
                            },
                            new TestLearningDelivery
                            {
                                LearnAimRef = "LearnAimRef2",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 4,
                            },
                        },
                    },
                    new TestLearner
                    {
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                LearnAimRef = "learnaimref3",
                                FworkCodeNullable = 1,
                                PwayCodeNullable = 3,
                            },
                        },
                    },
                    new TestLearner
                    {
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery(),
                        },
                    },
                },
            };
        }

        private LARSLearningDeliveryKeyMapper NewMapper()
        {
            return new LARSLearningDeliveryKeyMapper();
        }
    }
}
