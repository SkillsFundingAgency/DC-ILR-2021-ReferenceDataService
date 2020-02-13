using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR1819.DataStore.EF.Valid;
using ESFA.DC.ILR1819.DataStore.EF.Valid.Interface;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class FrmReferenceDataRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveFrmReferenceDataAsync()
        {
            var learnersList = new List<Learner>
            {
                new Learner
                {
                    UKPRN = 1,
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = "ValidAim",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 4,
                            FundModel = 25,
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByFundModel",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 4,
                            FundModel = 99,
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByProgType",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 3,
                            FundModel = 25,
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByLarsCategories",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 4,
                            FundModel = 25,
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByCompStatus",
                            CompStatus = 2,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 4,
                            FundModel = 25,
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByLearnPlanEndDate",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2019, 07, 31),
                            AimType = 4,
                            FundModel = 25,
                        }
                    }
                }
            };

            var larsList = new List<LarsLearningDelivery>
            {
                new LarsLearningDelivery()
                {
                    LearnAimRef = "VALIDAIM",
                    LarsLearningDeliveryCategories = new List<LarsLearningDeliveryCategory>
                    {
                        new LarsLearningDeliveryCategory
                        {
                            CategoryRef = 22
                        }
                    }
                },
                new LarsLearningDelivery()
                {
                    LearnAimRef = "ExcludedByLarsCategories",
                    LarsLearningDeliveryCategories = new List<LarsLearningDeliveryCategory>
                    {
                        new LarsLearningDeliveryCategory
                        {
                            CategoryRef = 23
                        }
                    }
                }
            };

            var learnerListDbMock = learnersList.AsQueryable().BuildMockDbSet();
            var larsListDbMock = larsList.AsQueryable().BuildMockDbSet();

            var ilrContextMock = new Mock<IILR1819_DataStoreEntitiesValid>();
            var ilrContextFactoryMock = new Mock<IDbContextFactory<IILR1819_DataStoreEntitiesValid>>();

            ilrContextMock.Setup(v => v.Learners).Returns(learnerListDbMock.Object);
            ilrContextFactoryMock.Setup(c => c.Create()).Returns(ilrContextMock.Object);

            var larsContextMock = new Mock<ILARSContext>();
            var larsContextFactoryMock = new Mock<IDbContextFactory<ILARSContext>>();

            larsContextMock.Setup(v => v.LARS_LearningDeliveries).Returns(larsListDbMock.Object);
            larsContextFactoryMock.Setup(c => c.Create()).Returns(larsContextMock.Object);

            var academicYearDataServiceMock = new Mock<IAcademicYearDataService>();

            academicYearDataServiceMock.SetupGet(s => s.CurrentYearStart).Returns(new DateTime(2019, 08, 01));

            var serviceResult = await NewService(ilrContextFactoryMock.Object, larsContextFactoryMock.Object, academicYearDataServiceMock.Object).RetrieveFrm06ReferenceDataAsync(1, CancellationToken.None);

            serviceResult.Count.Should().Be(1);

            var learner = serviceResult.First();
            learner.LearnAimRef.Should().Be("ValidAim");
            learner.AimType.Should().Be(4);
            learner.FundModel.Should().Be(25);
        }

        private FrmReferenceDataRepositoryService NewService(IDbContextFactory<IILR1819_DataStoreEntitiesValid> ilrContextFactory = null, IDbContextFactory<ILARSContext> larsContextFactory = null, IAcademicYearDataService academicYearDataService = null)
        {
            return new FrmReferenceDataRepositoryService(ilrContextFactory, larsContextFactory, academicYearDataService);
        }
    }
}
