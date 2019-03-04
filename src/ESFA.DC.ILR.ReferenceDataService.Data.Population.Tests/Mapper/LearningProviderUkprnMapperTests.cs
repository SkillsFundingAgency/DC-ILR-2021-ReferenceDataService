using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.ILR.ValidationService.Data.Population.Mapper.Message;
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

            NewMapper().MapFromMessage(message).Should().Be(99);
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapFromMessage(null).Should().Be(0);
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
