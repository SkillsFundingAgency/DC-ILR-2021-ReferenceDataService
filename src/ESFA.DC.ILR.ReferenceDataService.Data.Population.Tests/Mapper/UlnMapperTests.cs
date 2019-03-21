using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class UlnMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapUlnsFromMessage(message);

            mapper.Count().Should().Be(4);
            mapper.Should().Contain(new List<long> { 1, 2, 3, 4 });
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapUlnsFromMessage(null).Should().BeNullOrEmpty();
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        ULN = 1
                    },
                    new TestLearner
                    {
                        ULN = 2,
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

        private UlnMapper NewMapper()
        {
            return new UlnMapper();
        }
    }
}
