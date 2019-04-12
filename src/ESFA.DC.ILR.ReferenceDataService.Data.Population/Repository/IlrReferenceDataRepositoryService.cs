﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using Microsoft.EntityFrameworkCore;
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class IlrReferenceDataRepositoryService : IIlrReferenceDataRepositoryService
    {
        private const string Error = "E";
        private const string Warning = "W";
        private const string Fail = "F";
        private readonly IIlrReferenceDataContext _ilrReferenceDataContext;

        public IlrReferenceDataRepositoryService(IIlrReferenceDataContext ilrReferenceDataContext)
        {
            _ilrReferenceDataContext = ilrReferenceDataContext;
        }

        public async Task<IReadOnlyCollection<ValidationError>> RetrieveValidationErrorsAsync(CancellationToken cancellationToken)
        {
            return await _ilrReferenceDataContext.Rules
                .Select(r => new ValidationError
                {
                    RuleName = r.Rulename,
                    Severity = GetSeverity(r.Severity),
                    Message = r.Message
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Lookup>> RetrieveLookupsAsync(CancellationToken cancellationToken)
        {
            return await _ilrReferenceDataContext.Lookups
                .Include(l => l.LookupSubCategories)
                .Select(
                l => new Lookup
                {
                    Name = l.Name,
                    Code = l.Code,
                    EffectiveFrom = l.EffectiveFrom,
                    EffectiveTo = l.EffectiveTo,
                    SubCategories = l.LookupSubCategories
                    .Select(
                        sc => new LookupSubCategory
                        {
                            Code = sc.Code,
                            EffectiveFrom = sc.EffectiveFrom,
                            EffectiveTo = sc.EffectiveTo
                        }).ToList()
                }).ToListAsync(cancellationToken);
        }

        private SeverityLevel GetSeverity(string sev)
        {
            switch (sev)
            {
                case Error:
                    return SeverityLevel.Error;
                case Warning:
                    return SeverityLevel.Warning;
                case Fail:
                    return SeverityLevel.Fail;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sev));
            }
        }
    }
}