using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class StdCodeMapperTests
    {
        [Fact]
        public void UniqueSTDCodesFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapStandardCodesFromMessage(message);

            mapper.Count().Should().Be(3);
            mapper.Should().Contain(1);
            mapper.Should().Contain(2);
            mapper.Should().Contain(3);
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapStandardCodesFromMessage(null).Should().BeNullOrEmpty();
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
                                StdCodeNullable = 1,
                            },
                            new TestLearningDelivery
                            {
                                StdCodeNullable = 2,
                            },
                        },
                    },
                    new TestLearner
                    {
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                StdCodeNullable = 3,
                            },
                        },
                    },
                    new TestLearner
                    {
                        ULN = 3,
                    },
                },
            };
        }

        private StandardCodesMapper NewMapper()
        {
            return new StandardCodesMapper();
        }
    }
}
