using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class LearnAimRefMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapFromMessage(message);

            mapper.Count().Should().Be(3);
            mapper.Should().Contain(new List<string> { "LearnAimRef1", "LearnAimRef2", "LearnAimRef3" });
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapFromMessage(null).Should().BeNullOrEmpty();
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
                                LearnAimRef = "LearnAimRef1"
                            },
                            new TestLearningDelivery
                            {
                                LearnAimRef = "LearnAimRef2"
                            }
                        }
                    },
                    new TestLearner
                    {
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                LearnAimRef = "LearnAimRef3"
                            }
                        }
                    },
                    new TestLearner
                    {
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery()
                        }
                    }
                }
            };
        }

        private LearnAimRefMapper NewMapper()
        {
            return new LearnAimRefMapper();
        }
    }
}
