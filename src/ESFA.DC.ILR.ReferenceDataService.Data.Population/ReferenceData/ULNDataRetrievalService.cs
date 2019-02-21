using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.ULN;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IReadOnlyCollection<ULN>> RetrieveAsync(IReadOnlyCollection<long> ulns, CancellationToken cancellationToken)
        {
            return await
                _uln.UniqueLearnerNumbers
               .Where(u => ulns.Contains(u.Uln))
               .Batch(BatchSize).ToAsyncEnumerable()
               .SelectMany(u => u.Select(uln => new ULN { UniqueLearnerNumber = uln.Uln }))
               .ToList();
        }
    }
}
