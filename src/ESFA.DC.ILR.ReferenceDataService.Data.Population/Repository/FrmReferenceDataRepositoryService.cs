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
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class FrmReferenceDataRepositoryService : IFrmReferenceDataRepositoryService
    {
        private readonly IDbContextFactory<IILR1819_DataStoreEntitiesValid> _ilrContextFactory;
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;
        private readonly IAcademicYearDataService _academicYearDataService;

        private readonly int _excludedAimType = 3;
        private readonly int _excludedFundModel = 99;
        private readonly HashSet<int> _excludedCategories = new HashSet<int> { 23, 24, 27, 28, 29, 34, 35, 36 };

        public FrmReferenceDataRepositoryService(IDbContextFactory<IILR1819_DataStoreEntitiesValid> ilrContextFactory, IDbContextFactory<ILARSContext> larsContextFactory, IAcademicYearDataService academicYearDataService)
        {
            _ilrContextFactory = ilrContextFactory;
            _larsContextFactory = larsContextFactory;
            _academicYearDataService = academicYearDataService;
        }

        public async Task<IReadOnlyCollection<Frm06Learner>> RetrieveFrm06ReferenceDataAsync(long ukprn, CancellationToken cancellationToken)
        {
            var returnList = new List<Frm06Learner>();

            using (var larsContext = _larsContextFactory.Create())
            using (var context = _ilrContextFactory.Create())
            {
                var frmLearners = await context.Learners
                    .Where(l => l.UKPRN == ukprn)
                    .SelectMany(l => l.LearningDeliveries.Where(ld =>
                        ld.CompStatus == 1
                        && ld.LearnPlanEndDate >= _academicYearDataService.CurrentYearStart
                        && ld.AimType != _excludedAimType
                        && ld.FundModel != _excludedFundModel))
                    .Select(ld => new Frm06Learner
                    {
                        UKPRN = ld.UKPRN,
                        AimType = ld.AimType,
                        FundModel = ld.FundModel,
                        FworkCodeNullable = ld.FworkCode,
                        LearnAimRef = ld.LearnAimRef,
                        LearnRefNumber = ld.LearnRefNumber,
                        LearnStartDate = ld.LearnStartDate,
                        ProgTypeNullable = ld.ProgType,
                        StdCodeNullable = ld.StdCode
                    }).ToListAsync(cancellationToken);

                foreach (var learner in frmLearners)
                {
                    var excluded = await larsContext.LARS_LearningDeliveries
                        .Where(x =>
                            string.Equals(x.LearnAimRef, learner.LearnAimRef, StringComparison.OrdinalIgnoreCase)
                            && x.LarsLearningDeliveryCategories.Any(ldc => _excludedCategories.Contains(ldc.CategoryRef)))
                        .AnyAsync(cancellationToken);

                    if (!excluded)
                    {
                        returnList.Add(learner);
                    }
                }
            }

            return returnList;
        }
    }
}
