using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.ULNs;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class UlnRepositoryService : IUlnRepositoryService
    {
        private const int BatchSize = 5000;

        private readonly IUlnContext _uln;

        public UlnRepositoryService(IUlnContext uln)
        {
            _uln = uln;
        }

        public async Task<IReadOnlyCollection<long>> RetrieveAsync(IReadOnlyCollection<long> ulns, CancellationToken cancellationToken)
        {
            var result = new List<long>();

            var batches = ulns.Batch(BatchSize);

            foreach (var batch in batches)
            {
                result.AddRange(
                    await _uln.UniqueLearnerNumbers
                     .Where(u => batch.Contains(u.Uln))
                     .Select(u => u.Uln)
                     .ToListAsync(cancellationToken));
            }

            return result;
        }
    }
}
