using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class EpaOrgIdMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapEpaOrgIdsFromMessage(message);

            mapper.Count().Should().Be(3);
            mapper.Should().Contain(new List<string> { "EpaOrg1", "EpaOrg2", "EpaOrg3" });
            mapper.Should().NotContain("EpaOrg4");
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapEpaOrgIdsFromMessage(null).Should().BeNullOrEmpty();
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
                                EPAOrgID = "EpaOrg1"
                            },
                            new TestLearningDelivery
                            {
                                EPAOrgID = "EpaOrg2"
                            }
                        }
                    },
                    new TestLearner
                    {
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                EPAOrgID = "EpaOrg3"
                            }
                        }
                    },
                    new TestLearner
                    {
                        ULN = 3
                    }
                }
            };
        }

        private EpaOrgIdMapper NewMapper()
        {
            return new EpaOrgIdMapper();
        }
    }
}
