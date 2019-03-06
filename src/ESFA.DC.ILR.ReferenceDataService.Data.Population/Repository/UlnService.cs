﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.ULN;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class UlnService : IRetrievalService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>
    {
        private const int BatchSize = 5000;

        private readonly IUlnContext _uln;

        public UlnService(IUlnContext uln)
        {
            _uln = uln;
        }

        public async Task<IReadOnlyCollection<ULN>> RetrieveAsync(IReadOnlyCollection<long> ulns, CancellationToken cancellationToken)
        {
            var result = new List<ULN>();

            var batches = ulns.Batch(BatchSize);

            foreach (var batch in batches)
            {
                result.AddRange(
                    await _uln.UniqueLearnerNumbers
                     .Where(u => batch.Contains(u.Uln))
                     .Select(uln => new ULN { UniqueLearnerNumber = uln.Uln })
                     .ToListAsync(cancellationToken));
            }

            return result;
        }
    }
}