using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class AppEarningsHistoryRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<ApprenticeshipEarningsHistory>>
    {
        private const int BatchSize = 2500;
        private readonly IDbContextFactory<IAppEarnHistoryContext> _appEarnHistoryContextFactory;

        public AppEarningsHistoryRepositoryService(IDbContextFactory<IAppEarnHistoryContext> appEarnHistoryContextFactory)
        {
            _appEarnHistoryContextFactory = appEarnHistoryContextFactory;
        }

        public async Task<IReadOnlyCollection<ApprenticeshipEarningsHistory>> RetrieveAsync(IReadOnlyCollection<long> input, CancellationToken cancellationToken)
        {
            var appsHistory = new List<ApprenticeshipEarningsHistory>();

            var batches = input.Batch(BatchSize);

            using (var context = _appEarnHistoryContextFactory.Create())
            {
                foreach (var batch in batches)
                {
                    appsHistory.AddRange(
                        await context.AppsEarningsHistories
                          .Where(a => a.LatestInYear == true
                        && a.Uln < 9999999999
                        && batch.Contains(a.Uln))
                        .Select(aec => new ApprenticeshipEarningsHistory
                        {
                            AppIdentifier = aec.AppIdentifier,
                            AppProgCompletedInTheYearInput = aec.AppProgCompletedInTheYearInput,
                            CollectionYear = aec.CollectionYear,
                            CollectionReturnCode = aec.CollectionReturnCode,
                            DaysInYear = aec.DaysInYear,
                            FworkCode = aec.FworkCode,
                            HistoricEffectiveTNPStartDateInput = aec.HistoricEffectiveTnpstartDateInput,
                            HistoricEmpIdEndWithinYear = aec.HistoricEmpIdEndWithinYear,
                            HistoricEmpIdStartWithinYear = aec.HistoricEmpIdStartWithinYear,
                            HistoricLearner1618StartInput = aec.HistoricLearner1618StartInput,
                            HistoricPMRAmount = aec.HistoricPmramount,
                            HistoricTNP1Input = aec.HistoricTnp1input,
                            HistoricTNP2Input = aec.HistoricTnp2input,
                            HistoricTNP3Input = aec.HistoricTnp3input,
                            HistoricTNP4Input = aec.HistoricTnp4input,
                            HistoricTotal1618UpliftPaymentsInTheYearInput = aec.HistoricTotal1618UpliftPaymentsInTheYearInput,
                            HistoricVirtualTNP3EndOfTheYearInput = aec.HistoricVirtualTnp3endOfTheYearInput,
                            HistoricVirtualTNP4EndOfTheYearInput = aec.HistoricVirtualTnp4endOfTheYearInput,
                            HistoricLearnDelProgEarliestACT2DateInput = aec.HistoricLearnDelProgEarliestAct2dateInput,
                            LatestInYear = aec.LatestInYear,
                            LearnRefNumber = aec.LearnRefNumber,
                            ProgrammeStartDateIgnorePathway = aec.ProgrammeStartDateIgnorePathway,
                            ProgrammeStartDateMatchPathway = aec.ProgrammeStartDateMatchPathway,
                            ProgType = aec.ProgType,
                            PwayCode = aec.PwayCode,
                            STDCode = aec.Stdcode,
                            TotalProgAimPaymentsInTheYear = aec.TotalProgAimPaymentsInTheYear,
                            UptoEndDate = aec.UptoEndDate,
                            UKPRN = aec.Ukprn,
                            ULN = aec.Uln,
                        }).ToListAsync(cancellationToken));
                }

                return appsHistory;
            }
        }
    }
}
