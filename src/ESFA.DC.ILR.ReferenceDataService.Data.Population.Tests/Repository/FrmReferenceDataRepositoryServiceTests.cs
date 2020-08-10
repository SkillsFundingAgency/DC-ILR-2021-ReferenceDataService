using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR1920.DataStore.EF.Valid;
using ESFA.DC.ILR1920.DataStore.EF.Valid.Interface;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
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
                            Learner = new Learner
                            {
                                UKPRN = 1,
                                ULN = 1,
                                ProviderSpecLearnerMonitorings = new List<ProviderSpecLearnerMonitoring>(),
                            },
                            LearningDeliveryFAMs = new List<LearningDeliveryFAM>(),
                            ProviderSpecDeliveryMonitorings = new List<ProviderSpecDeliveryMonitoring>()
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByFundModel",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 4,
                            FundModel = 99,
                            Learner = new Learner
                            {
                                UKPRN = 1,
                                ULN = 1,
                                ProviderSpecLearnerMonitorings = new List<ProviderSpecLearnerMonitoring>(),
                            },
                            LearningDeliveryFAMs = new List<LearningDeliveryFAM>(),
                            ProviderSpecDeliveryMonitorings = new List<ProviderSpecDeliveryMonitoring>()
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByProgType",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 3,
                            FundModel = 25,
                            Learner = new Learner
                            {
                                UKPRN = 1,
                                ULN = 1,
                                ProviderSpecLearnerMonitorings = new List<ProviderSpecLearnerMonitoring>(),
                            },
                            LearningDeliveryFAMs = new List<LearningDeliveryFAM>(),
                            ProviderSpecDeliveryMonitorings = new List<ProviderSpecDeliveryMonitoring>()
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByLarsCategories",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 4,
                            FundModel = 25,
                            Learner = new Learner
                            {
                                UKPRN = 1,
                                ULN = 1,
                                ProviderSpecLearnerMonitorings = new List<ProviderSpecLearnerMonitoring>(),
                            },
                            LearningDeliveryFAMs = new List<LearningDeliveryFAM>(),
                            ProviderSpecDeliveryMonitorings = new List<ProviderSpecDeliveryMonitoring>()
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByCompStatus",
                            CompStatus = 2,
                            LearnPlanEndDate = new DateTime(2020, 01, 01),
                            AimType = 4,
                            FundModel = 25,
                            Learner = new Learner
                            {
                                UKPRN = 1,
                                ULN = 1,
                                ProviderSpecLearnerMonitorings = new List<ProviderSpecLearnerMonitoring>(),
                            },
                            LearningDeliveryFAMs = new List<LearningDeliveryFAM>(),
                            ProviderSpecDeliveryMonitorings = new List<ProviderSpecDeliveryMonitoring>()
                        },
                        new LearningDelivery
                        {
                            LearnAimRef = "ExcludedByLearnPlanEndDate",
                            CompStatus = 1,
                            LearnPlanEndDate = new DateTime(2019, 07, 31),
                            AimType = 4,
                            FundModel = 25,
                            Learner = new Learner
                            {
                                UKPRN = 1,
                                ULN = 1,
                                ProviderSpecLearnerMonitorings = new List<ProviderSpecLearnerMonitoring>(),
                            },
                            LearningDeliveryFAMs = new List<LearningDeliveryFAM>(),
                            ProviderSpecDeliveryMonitorings = new List<ProviderSpecDeliveryMonitoring>()
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

            var orgDetails = new List<OrgDetail>
            {
                new OrgDetail
                {
                    Ukprn = 1,
                    Name = "TestOrg"
                }
            };

            var learnerListDbMock = learnersList.AsQueryable().BuildMockDbSet();
            var larsListDbMock = larsList.AsQueryable().BuildMockDbSet();
            var orgListDbMock = orgDetails.AsQueryable().BuildMockDbSet();

            var ilrContextMock = new Mock<IILR1920_DataStoreEntitiesValid>();
            var ilrContextFactoryMock = new Mock<IDbContextFactory<IILR1920_DataStoreEntitiesValid>>();

            ilrContextMock.Setup(v => v.Learners).Returns(learnerListDbMock.Object);
            ilrContextFactoryMock.Setup(c => c.Create()).Returns(ilrContextMock.Object);

            var larsContextMock = new Mock<ILARSContext>();
            var larsContextFactoryMock = new Mock<IDbContextFactory<ILARSContext>>();

            larsContextMock.Setup(v => v.LARS_LearningDeliveries).Returns(larsListDbMock.Object);
            larsContextFactoryMock.Setup(c => c.Create()).Returns(larsContextMock.Object);

            var orgContextMock = new Mock<IOrganisationsContext>();
            var orgContextFactoryMock = new Mock<IDbContextFactory<IOrganisationsContext>>();

            orgContextMock.Setup(o => o.OrgDetails).Returns(orgListDbMock.Object);
            orgContextFactoryMock.Setup(c => c.Create()).Returns(orgContextMock.Object);

            var academicYearDataServiceMock = new Mock<IAcademicYearDataService>();

            academicYearDataServiceMock.SetupGet(s => s.CurrentYearStart).Returns(new DateTime(2019, 08, 01));

            var serviceResult = await NewService(ilrContextFactoryMock.Object, larsContextFactoryMock.Object, orgContextFactoryMock.Object, academicYearDataServiceMock.Object).RetrieveFrm06ReferenceDataAsync(1, CancellationToken.None);

            serviceResult.Count.Should().Be(1);

            var learner = serviceResult.First();
            learner.LearnAimRef.Should().Be("ValidAim");
            learner.AimType.Should().Be(4);
            learner.FundModel.Should().Be(25);
        }

        private FrmReferenceDataRepositoryService NewService(IDbContextFactory<IILR1920_DataStoreEntitiesValid> ilrContextFactory = null, IDbContextFactory<ILARSContext> larsContextFactory = null, IDbContextFactory<IOrganisationsContext> orgContextFactory = null, IAcademicYearDataService academicYearDataService = null)
        {
            return new FrmReferenceDataRepositoryService(ilrContextFactory, larsContextFactory, orgContextFactory, academicYearDataService);
        }
    }
}
