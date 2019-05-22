using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop.Repository
{
    public class DesktopEmployersRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>>
    {
        private const int BatchSize = 5000;
        private readonly IEmployersContext _employersContext;

        public DesktopEmployersRepositoryService(IEmployersContext employersContext)
        {
            _employersContext = employersContext;
        }

        public async Task<IReadOnlyCollection<Employer>> RetrieveAsync(CancellationToken cancellationToken)
        {
            var largeEmployers = await _employersContext.LargeEmployers.ToListAsync(cancellationToken);

            return
                largeEmployers.GroupBy(le => le.Ern)
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
