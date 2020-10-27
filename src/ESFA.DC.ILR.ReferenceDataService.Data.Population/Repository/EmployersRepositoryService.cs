using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EmployersRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>>
    {
        private const int BatchSize = 5000;
        private readonly IDbContextFactory<IEmployersContext> _employersContextFactory;

        public EmployersRepositoryService(IDbContextFactory<IEmployersContext> employersContextFactory)
        {
            _employersContextFactory = employersContextFactory;
        }

        public async Task<IReadOnlyCollection<Employer>> RetrieveAsync(IReadOnlyCollection<int> input, CancellationToken cancellationToken)
        {
            var edrsEmpIds = new List<int>();
            var largeEmployers = new List<ReferenceData.Employers.Model.LargeEmployer>();

            var batches = input.Batch(BatchSize);

            using (var context = _employersContextFactory.Create())
            {
                foreach (var batch in batches)
                {
                    edrsEmpIds.AddRange(
                        await context.Employers
                         .Where(e => batch.Contains(e.Urn))
                         .Select(e => e.Urn)
                         .ToListAsync(cancellationToken));

                    largeEmployers.AddRange(
                        await context.LargeEmployers
                         .Where(e => batch.Contains(e.Ern))
                         .ToListAsync(cancellationToken));
                }

                return
                    edrsEmpIds
                    .Select(empId => new Employer
                    {
                        ERN = empId,
                        LargeEmployerEffectiveDates = largeEmployers.Where(le => le.Ern == empId)
                        .Select(le => new LargeEmployerEffectiveDates
                        {
                            EffectiveFrom = le.EffectiveFrom,
                            EffectiveTo = le.EffectiveTo,
                        }).ToList(),
                    }).ToList();
            }
        }
    }
}
