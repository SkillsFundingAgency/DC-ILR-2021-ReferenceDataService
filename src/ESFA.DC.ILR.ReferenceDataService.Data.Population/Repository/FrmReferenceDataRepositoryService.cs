using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.FRM;
using ESFA.DC.ILR1819.DataStore.EF.Valid.Interface;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class FrmReferenceDataRepositoryService : IFrmReferenceDataRepositoryService
    {
        private readonly IDbContextFactory<IILR1819_DataStoreEntitiesValid> _ilrContextFactory;
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;
        private readonly IDbContextFactory<IOrganisationsContext> _orgContextFactory;
        private readonly IAcademicYearDataService _academicYearDataService;

        private readonly int _excludedAimType = 3;
        private readonly int _excludedFundModel = 99;
        private readonly HashSet<int> _excludedCategories = new HashSet<int> { 23, 24, 27, 28, 29, 34, 35, 36 };

        public FrmReferenceDataRepositoryService(IDbContextFactory<IILR1819_DataStoreEntitiesValid> ilrContextFactory, IDbContextFactory<ILARSContext> larsContextFactory, IDbContextFactory<IOrganisationsContext> orgContextFactory, IAcademicYearDataService academicYearDataService)
        {
            _ilrContextFactory = ilrContextFactory;
            _larsContextFactory = larsContextFactory;
            _orgContextFactory = orgContextFactory;
            _academicYearDataService = academicYearDataService;
        }

        public async Task<IReadOnlyCollection<FrmLearner>> RetrieveFrm06ReferenceDataAsync(long ukprn, CancellationToken cancellationToken)
        {
            var returnList = new List<FrmLearner>();

            using (var context = _ilrContextFactory.Create())
            {
                var frmLearners = await context.Learners
                    .Where(l => l.UKPRN == ukprn)
                    .SelectMany(l => l.LearningDeliveries.Where(ld =>
                        ld.CompStatus == 1
                        && ld.LearnPlanEndDate >= _academicYearDataService.CurrentYearStart
                        && ld.AimType != _excludedAimType
                        && ld.FundModel != _excludedFundModel))
                    .Select(ld => new FrmLearner
                    {
                        UKPRN = ld.UKPRN,
                        ULN = ld.Learner.ULN,
                        AimType = ld.AimType,
                        FundModel = ld.FundModel,
                        FworkCodeNullable = ld.FworkCode,
                        LearnAimRef = ld.LearnAimRef,
                        LearnRefNumber = ld.LearnRefNumber,
                        LearnStartDate = ld.LearnStartDate,
                        ProgTypeNullable = ld.ProgType,
                        StdCodeNullable = ld.StdCode,
                        PwayCodeNullable = ld.PwayCode,
                        AimSeqNumber = ld.AimSeqNumber,
                        PartnerUKPRN = ld.PartnerUKPRN,
                        PrevUKPRN = ld.Learner.PrevUKPRN,
                        PMUKPRN = ld.Learner.PMUKPRN,
                        PrevLearnRefNumber = ld.Learner.PrevLearnRefNumber,
                        CompStatus = ld.CompStatus,
                        Outcome = ld.Outcome,
                        PriorLearnFundAdj = ld.PriorLearnFundAdj,
                        OtherFundAdj = ld.OtherFundAdj,
                        LearnPlanEndDate = ld.LearnPlanEndDate,
                        LearnActEndDate = ld.LearnActEndDate,
                        SWSupAimId = ld.SWSupAimId,
                        LearningDeliveryFAMs = ld.LearningDeliveryFAMs
                            .Select(x =>
                                new LearningDeliveryFAM
                                {
                                    LearnDelFAMCode = x.LearnDelFAMCode,
                                    LearnDelFAMType = x.LearnDelFAMType,
                                    LearnDelFAMDateFrom = x.LearnDelFAMDateFrom,
                                    LearnDelFAMDateTo = x.LearnDelFAMDateTo
                                }).ToList(),
                        ProviderSpecLearnerMonitorings = ld.Learner.ProviderSpecLearnerMonitorings
                            .Select(lm =>
                                new ProviderSpecLearnerMonitoring
                                {
                                    ProvSpecLearnMon = lm.ProvSpecLearnMon,
                                    ProvSpecLearnMonOccur = lm.ProvSpecLearnMonOccur
                                }).ToList(),
                        ProvSpecDeliveryMonitorings = ld.ProviderSpecDeliveryMonitorings
                            .Select(dm => new ProviderSpecDeliveryMonitoring
                            {
                                ProvSpecDelMon = dm.ProvSpecDelMon,
                                ProvSpecDelMonOccur = dm.ProvSpecDelMonOccur
                            }).ToList()
                    }).ToListAsync(cancellationToken);

                var learnAimRefs = frmLearners.Select(l => l.LearnAimRef).Distinct();

                var larsLearningDeliveries = await RetrieveLarsLearningDeliveries(cancellationToken, learnAimRefs);

                foreach (var learner in frmLearners)
                {
                    var excluded = larsLearningDeliveries
                        .Any(x => string.Equals(x.LearnAimRef, learner.LearnAimRef, StringComparison.OrdinalIgnoreCase)
                            && x.LarsLearningDeliveryCategories.Any(ldc => _excludedCategories.Contains(ldc.CategoryRef)));

                    if (!excluded)
                    {
                        learner.LearningAimTitle = larsLearningDeliveries.FirstOrDefault(
                            x => string.Equals(x.LearnAimRef, learner.LearnAimRef, StringComparison.OrdinalIgnoreCase))?.LearnAimRefTitle;

                        returnList.Add(learner);
                    }
                }
            }

            await UpdateOrgNames(ukprn, cancellationToken, returnList);

            return returnList;
        }

        private async Task<List<LarsLearningDelivery>> RetrieveLarsLearningDeliveries(CancellationToken cancellationToken, IEnumerable<string> learnAimRefs)
        {
            using (var larsContext = _larsContextFactory.Create())
            {
                return await larsContext.LARS_LearningDeliveries.Where(ld => learnAimRefs.Contains(ld.LearnAimRef)).ToListAsync(cancellationToken);
            }
        }

        private async Task UpdateOrgNames(long ukprn, CancellationToken cancellationToken, List<FrmLearner> returnList)
        {
            using (var orgContext = _orgContextFactory.Create())
            {
                var partnerUKPRNs = returnList.Where(x => x.PartnerUKPRN.HasValue).Select(x => x.PartnerUKPRN.Value).Distinct()
                    .Union(new[] { ukprn });

                var orgNames = await orgContext.OrgDetails.Where(o => partnerUKPRNs.Contains(o.Ukprn))
                    .Select(n => new { UKPRN = n.Ukprn, OrgName = n.Name }).ToListAsync(cancellationToken);

                var orgName = orgNames.SingleOrDefault(o => o.UKPRN == ukprn)?.OrgName ?? string.Empty;

                foreach (var learner in returnList)
                {
                    learner.OrgName = orgName;
                    if (learner.PartnerUKPRN.HasValue)
                    {
                        learner.PartnerOrgName = orgNames.SingleOrDefault(o => o.UKPRN == learner.PartnerUKPRN)?.OrgName ?? string.Empty;
                    }
                }
            }
        }
    }
}
