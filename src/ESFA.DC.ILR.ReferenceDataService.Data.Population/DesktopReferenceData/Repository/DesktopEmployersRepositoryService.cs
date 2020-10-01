using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Constants;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopEmployersRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>>
    {
        private readonly IDbContextFactory<IEmployersContext> _employersContextFactory;
        private readonly IReferenceDataStatisticsService _referenceDataStatisticsService;

        public DesktopEmployersRepositoryService(
            IDbContextFactory<IEmployersContext> employersContextFactory,
            IReferenceDataStatisticsService referenceDataStatisticsService)
        {
            _employersContextFactory = employersContextFactory;
            _referenceDataStatisticsService = referenceDataStatisticsService;
        }

        public async Task<IReadOnlyCollection<Employer>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _employersContextFactory.Create())
            {
                var largeEmployers = await context.LargeEmployers.ToListAsync(cancellationToken);
                _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.LargeEmployers, largeEmployers.Count);

                return largeEmployers.GroupBy(le => le.Ern)
                    .Select(e => new Employer
                    {
                        ERN = e.Key,
                        LargeEmployerEffectiveDates = e.Select(le => new LargeEmployerEffectiveDates
                        {
                            EffectiveFrom = le.EffectiveFrom,
                            EffectiveTo = le.EffectiveTo,
                        }).ToList(),
                    }).ToList();
            }
        }
    }
}
