using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class UlnRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<long>>
    {
        private const int BatchSize = 5000;
        private readonly IDbContextFactory<IUlnContext> _ulnFactory;

        public UlnRepositoryService(IDbContextFactory<IUlnContext> ulnFactory)
        {
            _ulnFactory = ulnFactory;
        }

        public async Task<IReadOnlyCollection<long>> RetrieveAsync(IReadOnlyCollection<long> ulns, CancellationToken cancellationToken)
        {
            var result = new List<long>();

            var batches = ulns.Batch(BatchSize);

            using (var context = _ulnFactory.Create())
            {
                foreach (var batch in batches)
                {
                    result.AddRange(
                        await context.UniqueLearnerNumbers
                         .Where(u => batch.Contains(u.Uln))
                         .Select(u => u.Uln)
                         .ToListAsync(cancellationToken));
                }

                return result;
            }
        }
    }
}
