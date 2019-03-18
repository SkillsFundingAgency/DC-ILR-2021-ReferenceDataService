using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class UkprnsMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapFromMessage(message);

            mapper.Count().Should().Be(5);
            mapper.Should().Contain(new List<int> { 1, 2, 10, 20, 99 });
            mapper.Should().NotContain(4);
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            var mapper = NewMapper().MapFromMessage(null);

            mapper.Count().Should().Be(1);
            mapper.Should().Contain(new List<int> { 0 });
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = 99
                },
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        PMUKPRNNullable = 1,
                        PrevUKPRNNullable = 1,
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                PartnerUKPRNNullable = 10
                            }
                        }
                    },
                    new TestLearner
                    {
                        PMUKPRNNullable = 2,
                        PrevUKPRNNullable = 2,
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                PartnerUKPRNNullable = 20,
                            }
                        }
                    },
                    new TestLearner
                    {
                        ULN = 3
                    },
                },
                LearnerDestinationAndProgressions = new List<TestLearnerDestinationAndProgression>
                {
                    new TestLearnerDestinationAndProgression
                    {
                        ULN = 4
                    }
                }
            };
        }

        private UkprnsMapper NewMapper()
        {
            return new UkprnsMapper();
        }
    }
}
