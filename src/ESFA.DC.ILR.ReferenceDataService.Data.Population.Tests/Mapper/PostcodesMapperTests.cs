using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class PostcodesMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapPostcodesFromMessage(message);

            mapper.Count().Should().Be(6);
            mapper.Should().Contain(new List<string> { "Postcode1", "Postcode2", "PostcodePrior1", "PostcodePrior2", "DelLocPostCode1", "DelLocPostCode2" });
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapPostcodesFromMessage(null).Should().BeNullOrEmpty();
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        Postcode = "Postcode1",
                        PostcodePrior = "PostcodePrior1",
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                DelLocPostCode = "DelLocPostCode1"
                            }
                        }
                    },
                    new TestLearner
                    {
                        Postcode = "Postcode2",
                        PostcodePrior = "PostcodePrior2",
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                DelLocPostCode = "DelLocPostCode2"
                            }
                        }
                    },
                    new TestLearner()
                }
            };
        }

        private PostcodesMapper NewMapper()
        {
            return new PostcodesMapper();
        }
    }
}
