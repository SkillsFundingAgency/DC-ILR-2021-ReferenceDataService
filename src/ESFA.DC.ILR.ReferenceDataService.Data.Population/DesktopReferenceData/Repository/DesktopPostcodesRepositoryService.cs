﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopPostcodesRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>>
    {
        private readonly IDbContextFactory<IPostcodesContext> _postcodesContextFactory;
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IJsonSerializationService _jsonSerializationService;

        public DesktopPostcodesRepositoryService(
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IReferenceDataOptions referenceDataOptions,
            IJsonSerializationService jsonSerializationService)
        {
            _postcodesContextFactory = postcodesContextFactory;
            _referenceDataOptions = referenceDataOptions;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<IReadOnlyCollection<Postcode>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _postcodesContextFactory.Create())
            {
                var masterPostcodes = RetrieveMasterPostcodes(cancellationToken);
                var sfaAreaCosts = RetrieveSfaAreaCosts(cancellationToken);
                var sfaPostcodeDisadvantages = RetrieveSfaPostcodeDisadvantages(cancellationToken);
                var efaPostcodeDisadvantages = RetrieveEfaPostcodeDisadvantages(cancellationToken);
                var dasPostcodeDisadvantages = RetrieveDasPostcodeDisadvantages(cancellationToken);
                var onsData = RetrieveOnsData(cancellationToken);

                var taskList = new List<Task>
                {
                    masterPostcodes,
                    sfaAreaCosts,
                    sfaPostcodeDisadvantages,
                    efaPostcodeDisadvantages,
                    dasPostcodeDisadvantages,
                    onsData
                };

                await Task.WhenAll(taskList);

                return masterPostcodes.Result
                    .Select(postcode => new Postcode()
                    {
                        PostCode = postcode,
                        SfaDisadvantages = sfaPostcodeDisadvantages.Result.TryGetValue(postcode, out var sfaDisaValued) ? sfaDisaValued : null,
                        SfaAreaCosts = sfaAreaCosts.Result.TryGetValue(postcode, out var sfaAreaCostValue) ? sfaAreaCostValue : null,
                        DasDisadvantages = dasPostcodeDisadvantages.Result.TryGetValue(postcode, out var dasDisadValue) ? dasDisadValue : null,
                        EfaDisadvantages = efaPostcodeDisadvantages.Result.TryGetValue(postcode, out var efaDisadValue) ? efaDisadValue : null,
                        ONSData = onsData.Result.TryGetValue(postcode, out var onsValue) ? onsValue : null,
                    }).ToList();
            }
        }

        public async Task<List<string>> RetrieveMasterPostcodes(CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT [Postcode] FROM [dbo].[MasterPostcodes]";

            var postcodes = await RetrieveAsync<MasterPostcode>(sqlSfaAreaCost, cancellationToken);

            return postcodes
                  .Select(p => p.Postcode)
                  .ToList();
        }

        public async Task<IDictionary<string, List<SfaAreaCost>>> RetrieveSfaAreaCosts(CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT [Postcode], [AreaCostFactor], [EffectiveFrom], [EffectiveTo] 
                                                    FROM [dbo].[SFA_PostcodeAreaCost]";

            var sfaAReaCosts = await RetrieveAsync<SfaPostcodeAreaCost>(sqlSfaAreaCost, cancellationToken);

            return sfaAReaCosts
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(SfaAreaCostsToEntity).ToList());
        }

        public async Task<IDictionary<string, List<SfaDisadvantage>>> RetrieveSfaPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlSfaPostcodeDisadvantage = $@"SELECT [Postcode], [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[SFA_PostcodeDisadvantage]";

            var sfaDisadvantages = await RetrieveAsync<SfaPostcodeDisadvantage>(sqlSfaPostcodeDisadvantage, cancellationToken);

            return sfaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(SfaPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<EfaDisadvantage>>> RetrieveEfaPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlEfaPostcodeDisadvantage = $@"SELECT [Postcode], [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[EFA_PostcodeDisadvantage]";

            var efaDisadvantages = await RetrieveAsync<EfaPostcodeDisadvantage>(sqlEfaPostcodeDisadvantage, cancellationToken);

            return efaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(EfaPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<DasDisadvantage>>> RetrieveDasPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlDasPostcodeDisadvantage = $@"SELECT [Postcode], [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[DAS_PostcodeDisadvantage]";

            var dasDisadvantages = await RetrieveAsync<DasPostcodeDisadvantage>(sqlDasPostcodeDisadvantage, cancellationToken);

            return dasDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(DasPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<ONSData>>> RetrieveOnsData(CancellationToken cancellationToken)
        {
            var sqlOnsData = $@"SELECT [Postcode], [Introduction], [Termination], [LocalAuthority], [Lep1], [Lep2], 
                                                [EffectiveFrom], [EffectiveTo], [Nuts]
                                                FROM [dbo].[ONS_Postcodes]";

            var onsData = await RetrieveAsync<OnsPostcode>(sqlOnsData, cancellationToken);

            return onsData
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(ONSDataToEntity).ToList());
        }

        public virtual async Task<IEnumerable<T>> RetrieveAsync<T>(string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.PostcodesConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<T>(commandDefinition);
            }
        }

        public SfaDisadvantage SfaPostcodeDisadvantagesToEntity(SfaPostcodeDisadvantage sfaPostcodeDisadvantage)
        {
            return new SfaDisadvantage
            {
                Uplift = sfaPostcodeDisadvantage.Uplift,
                EffectiveFrom = sfaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = sfaPostcodeDisadvantage.EffectiveTo,
            };
        }

        public SfaAreaCost SfaAreaCostsToEntity(SfaPostcodeAreaCost sfaPostcodeAreaCost)
        {
            return new SfaAreaCost
            {
                AreaCostFactor = sfaPostcodeAreaCost.AreaCostFactor,
                EffectiveFrom = sfaPostcodeAreaCost.EffectiveFrom,
                EffectiveTo = sfaPostcodeAreaCost.EffectiveTo,
            };
        }

        public DasDisadvantage DasPostcodeDisadvantagesToEntity(DasPostcodeDisadvantage dasPostcodeDisadvantage)
        {
            return new DasDisadvantage
            {
                Uplift = dasPostcodeDisadvantage.Uplift,
                EffectiveFrom = dasPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = dasPostcodeDisadvantage.EffectiveTo,
            };
        }

        public EfaDisadvantage EfaPostcodeDisadvantagesToEntity(EfaPostcodeDisadvantage efaPostcodeDisadvantage)
        {
            return new EfaDisadvantage
            {
                Uplift = efaPostcodeDisadvantage.Uplift,
                EffectiveFrom = efaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = efaPostcodeDisadvantage.EffectiveTo,
            };
        }

        public ONSData ONSDataToEntity(OnsPostcode onsPostcode)
        {
            return new ONSData
            {
                EffectiveFrom = onsPostcode.EffectiveFrom,
                EffectiveTo = onsPostcode.EffectiveTo,
                Lep1 = onsPostcode.Lep1,
                Lep2 = onsPostcode.Lep2,
                LocalAuthority = onsPostcode.LocalAuthority,
                Nuts = onsPostcode.Nuts,
                Termination = GetEndOfMonthDateFromYearMonthString(onsPostcode.Termination),
            };
        }

        private DateTime? GetEndOfMonthDateFromYearMonthString(string yearMonth)
        {
            if (yearMonth == null || string.IsNullOrEmpty(yearMonth.Trim()) || yearMonth.Length != 6)
            {
                return null;
            }

            var yearParsed = int.TryParse(yearMonth.Substring(0, 4), out var year);
            var monthParsed = int.TryParse(yearMonth.Substring(4), out var month);

            if (!yearParsed || !monthParsed)
            {
                return null;
            }

            return new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
        }
    }
}
