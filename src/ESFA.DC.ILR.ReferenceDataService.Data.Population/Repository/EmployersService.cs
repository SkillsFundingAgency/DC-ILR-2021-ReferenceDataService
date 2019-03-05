using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ReferenceData.Employers.Model;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EmployersService : IRetrievalService<IReadOnlyCollection<Employers>, IReadOnlyCollection<int>>
    {
        private const int BatchSize = 5000;
        private readonly IEmployersContext _employersContext;

        public EmployersService(IEmployersContext employersContext)
        {
            _employersContext = employersContext;
        }

        public async Task<IReadOnlyCollection<Employers>> RetrieveAsync(IReadOnlyCollection<int> input, CancellationToken cancellationToken)
        {
            var edrsEmpIds = new List<int>();
            var largeEmployers = new List<LargeEmployer>();

            var result = new List<Employers>();

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
                .Select(empId => new Employers
                {
                    ERN = empId,
                    LargeEmployerEffectiveDates = LargeEmployerEffectiveDatesForEmpId(largeEmployers, empId)
                }).ToList();
        }

        public List<LargeEmployerEffectiveDates> LargeEmployerEffectiveDatesForEmpId(IEnumerable<LargeEmployer> largeEmployers, int empId)
        {
            if (largeEmployers == null)
            {
                return new List<LargeEmployerEffectiveDates>();
            }

            return
                largeEmployers?
                .Where(l => l.Ern == empId)
                .GroupBy(l => l.Ern)
                .SelectMany(e => e.Select(led => new LargeEmployerEffectiveDates
                {
                    EffectiveFrom = led.EffectiveFrom,
                    EffectiveTo = led.EffectiveTo
                })).ToList();
        }
    }
}
