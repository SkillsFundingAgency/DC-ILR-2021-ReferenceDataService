using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Mapper
{
    public class EmpIdMapperTests
    {
        [Fact]
        public void MapFromMessage()
        {
            var message = TestMessage();

            var mapper = NewMapper().MapEmpIdsFromMessage(message);

            mapper.Count().Should().Be(3);
            mapper.Should().Contain(new List<int> { 1, 2, 3 });
            mapper.Should().NotContain(4);
        }

        [Fact]
        public void MapFromMessage_NullMessage()
        {
            NewMapper().MapEmpIdsFromMessage(null).Should().BeNullOrEmpty();
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        LearnerEmploymentStatuses = new List<TestLearnerEmploymentStatus>
                        {
                            new TestLearnerEmploymentStatus
                            {
                                EmpIdNullable = 1,
                            },
                            new TestLearnerEmploymentStatus
                            {
                                EmpIdNullable = 2,
                            },
                        },
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                LearningDeliveryWorkPlacements = new List<TestLearningDeliveryWorkPlacement>
                                {
                                    new TestLearningDeliveryWorkPlacement
                                    {
                                        WorkPlaceEmpIdNullable = 3,
                                    },
                                },
                            },
                        },
                    },
                    new TestLearner
                    {
                        LearnerEmploymentStatuses = new List<TestLearnerEmploymentStatus>
                        {
                            new TestLearnerEmploymentStatus
                            {
                                EmpIdNullable = 2,
                            },
                        },
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                LearningDeliveryWorkPlacements = new List<TestLearningDeliveryWorkPlacement>
                                {
                                    new TestLearningDeliveryWorkPlacement
                                    {
                                        WorkPlaceEmpIdNullable = 2,
                                    },
                                },
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

        private EmpIdMapper NewMapper()
        {
            return new EmpIdMapper();
        }
    }
}
