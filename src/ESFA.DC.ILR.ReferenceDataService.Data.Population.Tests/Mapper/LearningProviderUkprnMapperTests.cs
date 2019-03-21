using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class LearningProviderUkprnMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            NewMapper().MapLearningProviderUKPRNFromMessage(message).Should().Be(99);
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapLearningProviderUKPRNFromMessage(null).Should().Be(0);
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = 99
                }
            };
        }

        private LearningProviderUkprnMapper NewMapper()
        {
            return new LearningProviderUkprnMapper();
        }
    }
}
