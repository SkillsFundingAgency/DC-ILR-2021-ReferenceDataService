using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EmployersRepositoryService : IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>>
    {
        private const int BatchSize = 5000;
        private readonly IEmployersContext _employersContext;

        public EmployersRepositoryService(IEmployersContext employersContext)
        {
            _employersContext = employersContext;
        }

        public async Task<IReadOnlyCollection<Employer>> RetrieveAsync(IReadOnlyCollection<int> input, CancellationToken cancellationToken)
        {
            var edrsEmpIds = new List<int>();
            var largeEmployers = new List<ReferenceData.Employers.Model.LargeEmployer>();

            var batches = input.Batch(BatchSize);

            foreach (var batch in batches)
            {
                edrsEmpIds.AddRange(
                    await _employersContext.Employers
                     .Where(e => input.Contains(e.Urn))
                     .Select(e => e.Urn)
                     .ToListAsync(cancellationToken));

                largeEmployers.AddRange(
                    await _employersContext.LargeEmployers
                     .Where(e => input.Contains(e.Ern))
                     .ToListAsync(cancellationToken));
            }

            return
                edrsEmpIds
                .Select(empId => new Employer
                {
                    ERN = empId,
                    LargeEmployerEffectiveDates = largeEmployers?.Where(le => le.Ern == empId)
                    .Select(le => new LargeEmployerEffectiveDates
                    {
                        EffectiveFrom = le.EffectiveFrom,
                        EffectiveTo = le.EffectiveTo,
                    }).ToList(),
                }).ToList();
        }
    }
}
