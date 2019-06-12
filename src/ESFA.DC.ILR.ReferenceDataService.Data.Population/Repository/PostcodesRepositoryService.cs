using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class PostcodesRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>>
    {
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IJsonSerializationService _jsonSerializationService;

        public PostcodesRepositoryService(IReferenceDataOptions referenceDataOptions, IJsonSerializationService jsonSerializationService)
        {
            _referenceDataOptions = referenceDataOptions;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<IReadOnlyCollection<Postcode>> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken)
        {
            var jsonParams = _jsonSerializationService.Serialize(input);

            var masterPostcodes = RetrieveMasterPostcodes(jsonParams, cancellationToken);
            var sfaAreaCosts = RetrieveSfaAreaCosts(jsonParams, cancellationToken);
            var sfaPostcodeDisadvantages = RetrieveSfaPostcodeDisadvantages(jsonParams, cancellationToken);
            var efaPostcodeDisadvantages = RetrieveEfaPostcodeDisadvantages(jsonParams, cancellationToken);
            var dasPostcodeDisadvantages = RetrieveDasPostcodeDisadvantages(jsonParams, cancellationToken);
            var careerLearningPilots = RetrieveCareerLearningPilots(jsonParams, cancellationToken);
            var onsData = RetrieveOnsData(jsonParams, cancellationToken);

            return masterPostcodes
                .Select(postcode => new Postcode()
                {
                    PostCode = postcode,
                    SfaDisadvantages = sfaPostcodeDisadvantages.TryGetValue(postcode, out var sfaDisad) ? sfaPostcodeDisadvantages[postcode] : null,
                    SfaAreaCosts = sfaAreaCosts.TryGetValue(postcode, out var sfaAreaCost) ? sfaAreaCosts[postcode] : null,
                    DasDisadvantages = dasPostcodeDisadvantages.TryGetValue(postcode, out var dasDisad) ? dasPostcodeDisadvantages[postcode] : null,
                    EfaDisadvantages = efaPostcodeDisadvantages.TryGetValue(postcode, out var efaDisad) ? efaPostcodeDisadvantages[postcode] : null,
                    CareerLearningPilots = careerLearningPilots.TryGetValue(postcode, out var pilot) ? careerLearningPilots[postcode] : null,
                    ONSData = onsData.TryGetValue(postcode, out var ons) ? onsData[postcode] : null
                }).ToList();
        }

        public List<string> RetrieveMasterPostcodes(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT P.[Postcode] FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                    INNER JOIN [dbo].[MasterPostcodes] J ON J.[Postcode] = P.[Postcode]";

            return RetrieveAsync<MasterPostcode>(jsonParams, sqlSfaAreaCost, cancellationToken).Result
                  .Select(p => p.Postcode)
                  .ToList();
        }

        public IDictionary<string, List<SfaAreaCost>> RetrieveSfaAreaCosts(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT P.[Postcode], J.[AreaCostFactor], J.[EffectiveFrom], J.[EffectiveTo] 
                                                    FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                    INNER JOIN [dbo].[SFA_PostcodeAreaCost] J ON J.[Postcode] = P.[Postcode]";

            return RetrieveAsync<SfaPostcodeAreaCost>(jsonParams, sqlSfaAreaCost, cancellationToken).Result
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(SfaAreaCostsToEntity).ToList());
        }

        public IDictionary<string, List<SfaDisadvantage>> RetrieveSfaPostcodeDisadvantages(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlSfaPostcodeDisadvantage = $@"SELECT P.[Postcode], J.[Uplift], J.[EffectiveFrom], J.[EffectiveTo] 
                                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                                INNER JOIN [dbo].[SFA_PostcodeDisadvantage] J ON J.[Postcode] = P.[Postcode]";

            return
                RetrieveAsync<SfaPostcodeDisadvantage>(jsonParams, sqlSfaPostcodeDisadvantage, cancellationToken).Result
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(SfaPostcodeDisadvantagesToEntity).ToList());
        }

        public IDictionary<string, List<EfaDisadvantage>> RetrieveEfaPostcodeDisadvantages(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlEfaPostcodeDisadvantage = $@"SELECT P.[Postcode], J.[Uplift], J.[EffectiveFrom], J.[EffectiveTo] 
                                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                                INNER JOIN [dbo].[EFA_PostcodeDisadvantage] J ON J.[Postcode] = P.[Postcode]";

            return
                RetrieveAsync<EfaPostcodeDisadvantage>(jsonParams, sqlEfaPostcodeDisadvantage, cancellationToken).Result
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(EfaPostcodeDisadvantagesToEntity).ToList());
        }

        public IDictionary<string, List<DasDisadvantage>> RetrieveDasPostcodeDisadvantages(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlDasPostcodeDisadvantage = $@"SELECT P.[Postcode], J.[Uplift], J.[EffectiveFrom], J.[EffectiveTo] 
                                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                                INNER JOIN [dbo].[DAS_PostcodeDisadvantage] J ON J.[Postcode] = P.[Postcode]";

            return
                RetrieveAsync<DasPostcodeDisadvantage>(jsonParams, sqlDasPostcodeDisadvantage, cancellationToken).Result
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(DasPostcodeDisadvantagesToEntity).ToList());
        }

        public IDictionary<string, List<CareerLearningPilot>> RetrieveCareerLearningPilots(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlcareerLearningPilots = $@"SELECT P.[Postcode], J.[AreaCode], J.[EffectiveFrom], J.[EffectiveTo] 
                                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                                INNER JOIN [dbo].[CareerLearningPilot_Postcode] J ON J.[Postcode] = P.[Postcode]";

            return
                 RetrieveAsync<CareerLearningPilotPostcode>(jsonParams, sqlcareerLearningPilots, cancellationToken).Result
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(CareerLearningPilotsToEntity).ToList());
        }

        public IDictionary<string, List<ONSData>> RetrieveOnsData(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlOnsData = $@"SELECT P.[Postcode], J.[Introduction], J.[Termination], J.[LocalAuthority], J.[Lep1], J.[Lep2], 
                                                J.[EffectiveFrom], J.[EffectiveTo], J.[Nuts], J.[Lsoa11]
                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                INNER JOIN [dbo].[ONS_Postcodes] J ON J.[Postcode] = P.[Postcode]";

            return
                RetrieveAsync<OnsPostcode>(jsonParams, sqlOnsData, cancellationToken).Result
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(ONSDataToEntity).ToList());
        }

        public virtual async Task<IEnumerable<T>> RetrieveAsync<T>(string jsonParams, string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.PostcodesConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, new { jsonParams }, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<T>(commandDefinition);
            }
        }

        public SfaDisadvantage SfaPostcodeDisadvantagesToEntity(SfaPostcodeDisadvantage sfaPostcodeDisadvantage)
        {
            return new SfaDisadvantage
            {
                Uplift = sfaPostcodeDisadvantage.Uplift,
                EffectiveFrom = sfaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = sfaPostcodeDisadvantage.EffectiveTo
            };
        }

        public SfaAreaCost SfaAreaCostsToEntity(SfaPostcodeAreaCost sfaPostcodeAreaCost)
        {
            return new SfaAreaCost
            {
                AreaCostFactor = sfaPostcodeAreaCost.AreaCostFactor,
                EffectiveFrom = sfaPostcodeAreaCost.EffectiveFrom,
                EffectiveTo = sfaPostcodeAreaCost.EffectiveTo
            };
        }

        public DasDisadvantage DasPostcodeDisadvantagesToEntity(DasPostcodeDisadvantage dasPostcodeDisadvantage)
        {
            return new DasDisadvantage
            {
                Uplift = dasPostcodeDisadvantage.Uplift,
                EffectiveFrom = dasPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = dasPostcodeDisadvantage.EffectiveTo
            };
        }

        public EfaDisadvantage EfaPostcodeDisadvantagesToEntity(EfaPostcodeDisadvantage efaPostcodeDisadvantage)
        {
            return new EfaDisadvantage
            {
                Uplift = efaPostcodeDisadvantage.Uplift,
                EffectiveFrom = efaPostcodeDisadvantage.EffectiveFrom,
                EffectiveTo = efaPostcodeDisadvantage.EffectiveTo
            };
        }

        public CareerLearningPilot CareerLearningPilotsToEntity(CareerLearningPilotPostcode careerLearningPilot)
        {
            return new CareerLearningPilot
            {
                AreaCode = careerLearningPilot.AreaCode,
                EffectiveFrom = careerLearningPilot.EffectiveFrom,
                EffectiveTo = careerLearningPilot.EffectiveTo
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
                Termination = GetEndOfMonthDateFromYearMonthString(onsPostcode.Termination)
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
