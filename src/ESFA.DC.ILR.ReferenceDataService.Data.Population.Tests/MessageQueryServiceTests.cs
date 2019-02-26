using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.PrePopulation;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests
{
    public class MessageQueryServiceTests
    {
        [Fact]
        public void UniqueSTDCodesFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueSTDCodesFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain(1);
            service.Should().Contain(2);
        }

        [Fact]
        public void GetLearnerEmpIdsFromFile()
        {
            var message = TestMessage();

            var service = NewService().GetLearnerEmpIdsFromFile(message);

            service.Count().Should().Be(2);
            service.Should().Contain(1);
            service.Should().Contain(2);
        }

        [Fact]
        public void GetWorkplaceEmpIdsFromFile()
        {
            var message = TestMessage();

            var service = NewService().GetWorkplaceEmpIdsFromFile(message);

            service.Count().Should().Be(2);
            service.Should().Contain(1);
            service.Should().Contain(2);
        }

        [Fact]
        public void UniqueEmployerIdsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueEmployerIdsFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain(1);
            service.Should().Contain(2);
        }

        [Fact]
        public void UniqueLearnerPostcodesFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearnerPostcodesFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain("Postcode1");
            service.Should().Contain("Postcode2");
        }

        [Fact]
        public void UniqueLearnerPostcodePriorsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearnerPostcodePriorsFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain("PostcodePrior1");
            service.Should().Contain("PostcodePrior2");
        }

        [Fact]
        public void GetWorkplaceEmpIdUniqueLearningDeliveryLocationPostcodesFromMessagesFromFile()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearningDeliveryLocationPostcodesFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain("DelLocPostCode1");
            service.Should().Contain("DelLocPostCode2");
        }

        [Fact]
        public void UniquePostcodesFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniquePostcodesFromMessage(message);

            service.Count().Should().Be(6);
            service.Should().Contain(new List<string> { "Postcode1", "Postcode2", "PostcodePrior1", "PostcodePrior2", "DelLocPostCode1", "DelLocPostCode2" });
        }

        [Fact]
        public void UniqueULNsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueULNsFromMessage(message);

            service.Count().Should().Be(4);
            service.Should().Contain(new List<long> { 1, 2, 3, 4 });
        }

        [Fact]
        public void UniqueLearnAimRefsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearnAimRefsFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain(new List<string> { "LearnAimRef1", "LearnAimRef2" });
        }

        [Fact]
        public void UniqueEpaOrgIdsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueEpaOrgIdsFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain(new List<string> { "EPAOrgId1", "EPAOrgId2" });
        }

        [Fact]
        public void UniqueLearningProviderUKPRNFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearningProviderUKPRNFromMessage(message);

            service.Count().Should().Be(1);
            service.Should().Contain(99);
        }

        [Fact]
        public void UniqueLearnerPrevUKPRNsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearnerPrevUKPRNsFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain(new List<int> { 1, 2 });
        }

        [Fact]
        public void UniqueLearnerPMUKPRNsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearnerPMUKPRNsFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain(new List<int> { 1, 2 });
        }

        [Fact]
        public void UniqueLearningDeliveryPartnerUKPRNsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueLearningDeliveryPartnerUKPRNsFromMessage(message);

            service.Count().Should().Be(2);
            service.Should().Contain(new List<int> { 10, 20 });
        }

        [Fact]
        public void UniqueUKPRNsFromMessage()
        {
            var message = TestMessage();

            var service = NewService().UniqueUKPRNsFromMessage(message);

            service.Count().Should().Be(5);
            service.Should().Contain(new List<int> { 1, 2, 10, 20, 99 });
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
                        ULN = 1,
                        Postcode = "Postcode1",
                        PostcodePrior = "PostcodePrior1",
                        PMUKPRNNullable = 1,
                        PrevUKPRNNullable = 1,
                        LearnerEmploymentStatuses = new List<TestLearnerEmploymentStatus>
                        {
                            new TestLearnerEmploymentStatus
                            {
                                EmpIdNullable = 1
                            }
                        },
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                DelLocPostCode = "DelLocPostCode1",
                                LearnAimRef = "LearnAimRef1",
                                PartnerUKPRNNullable = 10,
                                EPAOrgID = "EPAOrgId1",
                                StdCodeNullable = 1,
                                LearningDeliveryWorkPlacements = new List<TestLearningDeliveryWorkPlacement>
                                {
                                    new TestLearningDeliveryWorkPlacement
                                    {
                                        WorkPlaceEmpIdNullable = 1
                                    }
                                }
                            }
                        }
                    },
                    new TestLearner
                    {
                        ULN = 2,
                        Postcode = "Postcode2",
                        PostcodePrior = "PostcodePrior2",
                        PMUKPRNNullable = 2,
                        PrevUKPRNNullable = 2,
                        LearnerEmploymentStatuses = new List<TestLearnerEmploymentStatus>
                        {
                            new TestLearnerEmploymentStatus
                            {
                                EmpIdNullable = 2
                            }
                        },
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                DelLocPostCode = "DelLocPostCode2",
                                LearnAimRef = "LearnAimRef2",
                                PartnerUKPRNNullable = 20,
                                EPAOrgID = "EPAOrgId2",
                                StdCodeNullable = 2,
                                LearningDeliveryWorkPlacements = new List<TestLearningDeliveryWorkPlacement>
                                {
                                    new TestLearningDeliveryWorkPlacement
                                    {
                                        WorkPlaceEmpIdNullable = 2
                                    }
                                }
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

        private MessageQueryService NewService()
        {
            return new MessageQueryService();
        }
    }
}
