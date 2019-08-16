using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class AppEarningsHistoryRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var ulns = new List<long> { 10000000, 20000000, 30000000 };

            IEnumerable<AppsEarningsHistory> appHistoriesList = new List<AppsEarningsHistory>
            {
                new AppsEarningsHistory
                {
                     AppIdentifier = "AppIdentifier_1_10000000",
                     AppProgCompletedInTheYearInput = true,
                     CollectionYear = "1920",
                     CollectionReturnCode = "R01",
                     DaysInYear = 1,
                     FworkCode = 2,
                     HistoricEffectiveTnpstartDateInput = new DateTime(2018, 8, 1),
                     HistoricEmpIdEndWithinYear = 2,
                     HistoricEmpIdStartWithinYear = 3,
                     HistoricLearner1618StartInput = true,
                     HistoricPmramount = 4.0m,
                     HistoricTnp1input = 5.0m,
                     HistoricTnp2input = 6.0m,
                     HistoricTnp3input = 7.0m,
                     HistoricTnp4input = 8.0m,
                     HistoricTotal1618UpliftPaymentsInTheYearInput = 9.0m,
                     HistoricVirtualTnp3endOfTheYearInput = 10.0m,
                     HistoricVirtualTnp4endOfTheYearInput = 11.0m,
                     HistoricLearnDelProgEarliestAct2dateInput = new DateTime(2018, 8, 1),
                     LatestInYear = true,
                     LearnRefNumber = "LearnRefNumber1",
                     ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                     ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                     ProgType = 12,
                     PwayCode = 13,
                     Stdcode = 14,
                     TotalProgAimPaymentsInTheYear = 15.0m,
                     UptoEndDate = new DateTime(2018, 8, 1),
                     Ukprn = 16,
                     Uln = 10000000,
                },
                new AppsEarningsHistory
                {
                     AppIdentifier = "AppIdentifier_2_10000000",
                     AppProgCompletedInTheYearInput = true,
                     CollectionYear = "1920",
                     CollectionReturnCode = "R01",
                     DaysInYear = 1,
                     FworkCode = 2,
                     HistoricEffectiveTnpstartDateInput = new DateTime(2018, 8, 1),
                     HistoricEmpIdEndWithinYear = 2,
                     HistoricEmpIdStartWithinYear = 3,
                     HistoricLearner1618StartInput = true,
                     HistoricPmramount = 4.0m,
                     HistoricTnp1input = 5.0m,
                     HistoricTnp2input = 6.0m,
                     HistoricTnp3input = 7.0m,
                     HistoricTnp4input = 8.0m,
                     HistoricTotal1618UpliftPaymentsInTheYearInput = 9.0m,
                     HistoricVirtualTnp3endOfTheYearInput = 10.0m,
                     HistoricVirtualTnp4endOfTheYearInput = 11.0m,
                     HistoricLearnDelProgEarliestAct2dateInput = new DateTime(2018, 8, 1),
                     LatestInYear = true,
                     LearnRefNumber = "LearnRefNumber1",
                     ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                     ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                     ProgType = 12,
                     PwayCode = 13,
                     Stdcode = 14,
                     TotalProgAimPaymentsInTheYear = 15.0m,
                     UptoEndDate = new DateTime(2018, 8, 1),
                     Ukprn = 16,
                     Uln = 10000000,
                },
                new AppsEarningsHistory
                {
                     AppIdentifier = "AppIdentifier_1_20000000",
                     AppProgCompletedInTheYearInput = true,
                     CollectionYear = "1920",
                     CollectionReturnCode = "R01",
                     DaysInYear = 1,
                     FworkCode = 2,
                     HistoricEffectiveTnpstartDateInput = new DateTime(2018, 8, 1),
                     HistoricEmpIdEndWithinYear = 2,
                     HistoricEmpIdStartWithinYear = 3,
                     HistoricLearner1618StartInput = true,
                     HistoricPmramount = 4.0m,
                     HistoricTnp1input = 5.0m,
                     HistoricTnp2input = 6.0m,
                     HistoricTnp3input = 7.0m,
                     HistoricTnp4input = 8.0m,
                     HistoricTotal1618UpliftPaymentsInTheYearInput = 9.0m,
                     HistoricVirtualTnp3endOfTheYearInput = 10.0m,
                     HistoricVirtualTnp4endOfTheYearInput = 11.0m,
                     HistoricLearnDelProgEarliestAct2dateInput = new DateTime(2018, 8, 1),
                     LatestInYear = true,
                     LearnRefNumber = "LearnRefNumber1",
                     ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                     ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                     ProgType = 12,
                     PwayCode = 13,
                     Stdcode = 14,
                     TotalProgAimPaymentsInTheYear = 15.0m,
                     UptoEndDate = new DateTime(2018, 8, 1),
                     Ukprn = 16,
                     Uln = 20000000,
                },
            };

            var appHistoriesMock = appHistoriesList.AsQueryable().BuildMockDbSet();

            var appHistoryMock = new Mock<IAppEarnHistoryContext>();
            appHistoryMock.Setup(a => a.AppsEarningsHistories).Returns(appHistoriesMock.Object);

            var appHistoryContextFactoryMock = new Mock<IDbContextFactory<IAppEarnHistoryContext>>();
            appHistoryContextFactoryMock.Setup(c => c.Create()).Returns(appHistoryMock.Object);

            var appHistoryResult = await NewService(appHistoryContextFactoryMock.Object).RetrieveAsync(ulns, CancellationToken.None);

            appHistoryResult.Should().HaveCount(3);
            appHistoryResult.Select(u => u.ULN).Should().Contain(10000000);
            appHistoryResult.Select(u => u.ULN).Should().Contain(20000000);
            appHistoryResult.Select(u => u.ULN).Should().NotContain(30000000);

            appHistoryResult.Where(u => u.ULN == 10000000).Should().HaveCount(2);
            appHistoryResult.Where(u => u.ULN == 20000000).Should().HaveCount(1);

            appHistoryResult.Where(u => u.ULN == 10000000).Select(e => e.AppIdentifier).Should().Contain("AppIdentifier_1_10000000", "AppIdentifier_2_10000000");
            appHistoryResult.Where(u => u.ULN == 20000000).Select(e => e.AppIdentifier).Should().Contain("AppIdentifier_1_20000000");
        }

        private AppEarningsHistoryRepositoryService NewService(IDbContextFactory<IAppEarnHistoryContext> appHistoryContextFactory = null)
        {
            return new AppEarningsHistoryRepositoryService(appHistoryContextFactory);
        }
    }
}
