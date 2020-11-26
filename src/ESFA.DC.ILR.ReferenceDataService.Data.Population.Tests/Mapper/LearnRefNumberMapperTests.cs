using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class LearnRefNumberMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapLearnRefNumbersFromMessage(message);

            mapper.Count().Should().Be(5);
            mapper.Should().Contain(new List<string> { "Learner1", "Learner2", "Learner3", "PrevLearner1", "PrevLearner2" });
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            var mapper = NewMapper().MapLearnRefNumbersFromMessage(null);

            mapper.Count().Should().Be(0);
            mapper.Should().BeNullOrEmpty();
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        LearnRefNumber = "Learner1",
                        PrevLearnRefNumber = "PrevLearner1",
                    },
                    new TestLearner
                    {
                        LearnRefNumber = "Learner2",
                        PrevLearnRefNumber = "PrevLearner2",
                    },
                    new TestLearner
                    {
                        LearnRefNumber = "Learner3",
                        ULN = 3,
                    },
                }
            };
        }

        private LearnRefNumberMapper NewMapper()
        {
            return new LearnRefNumberMapper();
        }
    }
}
