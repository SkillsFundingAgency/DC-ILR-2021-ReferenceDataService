﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class DevolvedPostcodesRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<string>, DevolvedPostcodes>
    {
        private readonly IPostcodesContext _postcodesContext;

        public DevolvedPostcodesRepositoryService(IPostcodesContext postcodesContext)
        {
            _postcodesContext = postcodesContext;
        }

        public async Task<DevolvedPostcodes> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken)
        {
            // TODO: input parameter to be used in story  84296 to return

            var mcaGlaFullNames = await _postcodesContext.McaglaFullNames?
                .Where(e => e.EffectiveTo == null)
                .ToDictionaryAsync(
                k => k.McaglaShortCode,
                v => v.FullName,
                StringComparer.OrdinalIgnoreCase,
                cancellationToken);

            var mcaGlaSofCodes = await _postcodesContext.McaglaSofs?.ToListAsync(cancellationToken);

            var mcaGlaSofLookups = mcaGlaSofCodes.Select(m => new McaGlaSofLookup
            {
                SofCode = m.SofCode,
                McaGlaShortCode = m.McaglaShortCode,
                McaGlaFullName = mcaGlaFullNames.TryGetValue(m.McaglaShortCode, out var fullname) ? fullname : string.Empty,
                EffectiveFrom = m.EffectiveFrom,
                EffectiveTo = m.EffectiveTo
            }).ToList();

            return new DevolvedPostcodes
            {
                McaGlaSofLookups = mcaGlaSofLookups
            };
        }
    }
}