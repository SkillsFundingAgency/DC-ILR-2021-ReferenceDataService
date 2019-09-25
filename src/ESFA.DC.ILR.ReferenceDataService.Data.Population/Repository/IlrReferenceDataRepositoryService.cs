using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;
using Microsoft.EntityFrameworkCore;
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;
using Lookup = ESFA.DC.ILR.ReferenceDataService.Model.MetaData.Lookup;
using LookupSubCategory = ESFA.DC.ILR.ReferenceDataService.Model.MetaData.LookupSubCategory;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class IlrReferenceDataRepositoryService : IIlrReferenceDataRepositoryService
    {
        private const string Error = "E";
        private const string Warning = "W";
        private const string Fail = "F";
        private readonly IDbContextFactory<IIlrReferenceDataContext> _ilrReferenceDataContextFactory;
        private readonly IBulkInsert _bulkInsert;

        public IlrReferenceDataRepositoryService(IDbContextFactory<IIlrReferenceDataContext> ilrReferenceDataContextFactory, IBulkInsert bulkInsert)
        {
            _ilrReferenceDataContextFactory = ilrReferenceDataContextFactory;
            _bulkInsert = bulkInsert;
        }

        public async Task<MetaData> RetrieveIlrReferenceDataAsync(CancellationToken cancellationToken)
        {
            using (var context = _ilrReferenceDataContextFactory.Create())
            {
                return new MetaData
                {
                    ValidationErrors = await RetrieveValidationErrorsAsync(context, cancellationToken) ?? Array.Empty<ValidationError>(),
                    ValidationRules = await RetrieveValidationRulesAsync(context, cancellationToken) ?? Array.Empty<ValidationRule>(),
                    Lookups = await RetrieveLookupsAsync(context, cancellationToken) ?? Array.Empty<Lookup>(),
                    CollectionDates = RetrieveCollectionDates()
                };
            }
        }

        public async Task ClearValidationRules(SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken)
        {
            const string sqlCommand = "DELETE FROM [dbo].[Rules]";

            using (SqlCommand command = new SqlCommand(sqlCommand, sqlConnection, sqlTransaction))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public async Task WriteValidationRules(IEnumerable<Rule> rules, SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken)
        {
            await _bulkInsert.Insert("Rules", rules, sqlConnection, sqlTransaction, cancellationToken);
        }

        private async Task<IReadOnlyCollection<ValidationError>> RetrieveValidationErrorsAsync(IIlrReferenceDataContext context, CancellationToken cancellationToken)
        {
            return await context.Rules?
            .Select(r => new ValidationError
            {
                RuleName = r.Rulename,
                Severity = GetSeverity(r.Severity),
                Message = r.Message,
            })
            .ToListAsync(cancellationToken);
        }

        private async Task<IReadOnlyCollection<ValidationRule>> RetrieveValidationRulesAsync(IIlrReferenceDataContext context, CancellationToken cancellationToken)
        {
            return await context.Rules?
                .Select(r => new ValidationRule
                {
                    RuleName = r.Rulename,
                    Desktop = r.Desktop.Value,
                    Online = r.Online.Value,
                })
                .ToListAsync(cancellationToken);
        }

        private async Task<IReadOnlyCollection<Lookup>> RetrieveLookupsAsync(IIlrReferenceDataContext context, CancellationToken cancellationToken)
        {
            return await context.Lookups
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
                            EffectiveTo = sc.EffectiveTo,
                        }).ToList(),
                }).ToListAsync(cancellationToken);
        }

        private IlrCollectionDates RetrieveCollectionDates()
        {
            return new IlrCollectionDates
            {
                CensusDates = CensusDates(),
                ReturnPeriods = ReturnPeriods()
            };
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

        private List<ReturnPeriod> ReturnPeriods()
        {
            return new List<ReturnPeriod>
            {
                new ReturnPeriod { Name = "R01", Period = 1, Start = new DateTime(2019, 08, 22), End = new DateTime(2019, 09, 05, 23, 59, 59) },
                new ReturnPeriod { Name = "R02", Period = 2, Start = new DateTime(2019, 09, 06), End = new DateTime(2019, 10, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R03", Period = 3, Start = new DateTime(2019, 10, 05), End = new DateTime(2019, 11, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R04", Period = 4, Start = new DateTime(2019, 11, 07), End = new DateTime(2019, 12, 05, 23, 59, 59) },
                new ReturnPeriod { Name = "R05", Period = 5, Start = new DateTime(2019, 12, 06), End = new DateTime(2020, 01, 07, 23, 59, 59) },
                new ReturnPeriod { Name = "R06", Period = 6, Start = new DateTime(2020, 01, 08), End = new DateTime(2020, 02, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R07", Period = 7, Start = new DateTime(2020, 02, 07), End = new DateTime(2020, 03, 05, 23, 59, 59) },
                new ReturnPeriod { Name = "R08", Period = 8, Start = new DateTime(2020, 03, 06), End = new DateTime(2020, 04, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R09", Period = 9, Start = new DateTime(2020, 04, 07), End = new DateTime(2020, 05, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R10", Period = 10, Start = new DateTime(2020, 05, 07), End = new DateTime(2020, 06, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R11", Period = 11, Start = new DateTime(2020, 06, 05), End = new DateTime(2020, 07, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R12", Period = 12, Start = new DateTime(2020, 07, 07), End = new DateTime(2020, 08, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R13", Period = 13, Start = new DateTime(2020, 08, 07), End = new DateTime(2020, 09, 14, 23, 59, 59) },
                new ReturnPeriod { Name = "R14", Period = 14, Start = new DateTime(2020, 09, 15), End = new DateTime(2020, 10, 22, 23, 59, 59) },
            };
        }

        private List<CensusDate> CensusDates()
        {
            return new List<CensusDate>
            {
                new CensusDate { Period = 01, Start = new DateTime(2019, 08, 01), End = new DateTime(2019, 08, 31, 23, 59, 59) },
                new CensusDate { Period = 02, Start = new DateTime(2019, 09, 01), End = new DateTime(2019, 09, 30, 23, 59, 59) },
                new CensusDate { Period = 03, Start = new DateTime(2019, 10, 01), End = new DateTime(2019, 10, 31, 23, 59, 59) },
                new CensusDate { Period = 04, Start = new DateTime(2019, 11, 01), End = new DateTime(2019, 11, 30, 23, 59, 59) },
                new CensusDate { Period = 05, Start = new DateTime(2019, 12, 01), End = new DateTime(2019, 12, 31, 23, 59, 59) },
                new CensusDate { Period = 06, Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 31, 23, 59, 59) },
                new CensusDate { Period = 07, Start = new DateTime(2020, 02, 01), End = new DateTime(2020, 02, 29, 23, 59, 59) },
                new CensusDate { Period = 08, Start = new DateTime(2020, 03, 01), End = new DateTime(2020, 03, 31, 23, 59, 59) },
                new CensusDate { Period = 09, Start = new DateTime(2020, 04, 01), End = new DateTime(2020, 04, 30, 23, 59, 59) },
                new CensusDate { Period = 10, Start = new DateTime(2020, 05, 01), End = new DateTime(2020, 05, 31, 23, 59, 59) },
                new CensusDate { Period = 11, Start = new DateTime(2020, 06, 01), End = new DateTime(2020, 06, 30, 23, 59, 59) },
                new CensusDate { Period = 12, Start = new DateTime(2020, 07, 01), End = new DateTime(2020, 07, 31, 23, 59, 59) },
            };
        }
    }
}
