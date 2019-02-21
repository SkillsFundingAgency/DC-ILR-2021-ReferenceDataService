using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.ReferenceData
{
    public class ULNDataRetrievalService : IULNDataRetrievalService
    {
        private const int BatchSize = 5;

        private readonly IUlnContext _uln;

        public ULNDataRetrievalService(IUlnContext uln)
        {
            _uln = uln;
        }

        public async Task<IReadOnlyCollection<long>> RetrieveAsync(IReadOnlyCollection<long> ulns, CancellationToken cancellationToken)
        {
            return await
                _uln.UniqueLearnerNumbers
               .Where(u => ulns.Contains(u.Uln))
               .Batch(BatchSize).ToAsyncEnumerable()
               .SelectMany(u => u.Select(uln => uln.Uln))
               .ToList();
        }
    }
}
