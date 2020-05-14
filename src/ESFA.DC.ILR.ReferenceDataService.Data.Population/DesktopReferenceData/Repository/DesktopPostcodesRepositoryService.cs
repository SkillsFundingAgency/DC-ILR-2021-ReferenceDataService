using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
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
        private readonly IPostcodesEntityModelMapper _postcodesEntityModelMapper;

        public DesktopPostcodesRepositoryService(
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IReferenceDataOptions referenceDataOptions,
            IJsonSerializationService jsonSerializationService,
            IPostcodesEntityModelMapper postcodesEntityModelMapper)
        {
            _postcodesContextFactory = postcodesContextFactory;
            _referenceDataOptions = referenceDataOptions;
            _jsonSerializationService = jsonSerializationService;
            _postcodesEntityModelMapper = postcodesEntityModelMapper;
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
                var specialistResources = RetrieveSpecialistResources(cancellationToken);
                var onsData = RetrieveOnsData(cancellationToken);

                var taskList = new List<Task>
                {
                    masterPostcodes,
                    sfaAreaCosts,
                    sfaPostcodeDisadvantages,
                    efaPostcodeDisadvantages,
                    dasPostcodeDisadvantages,
                    specialistResources,
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
                        PostcodeSpecialistResources = specialistResources.Result.TryGetValue(postcode, out var specResValue) ? specResValue : null,
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
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.SfaAreaCostsToEntity).ToList());
        }

        public async Task<IDictionary<string, List<SfaDisadvantage>>> RetrieveSfaPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlSfaPostcodeDisadvantage = $@"SELECT [Postcode], [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[SFA_PostcodeDisadvantage]";

            var sfaDisadvantages = await RetrieveAsync<SfaPostcodeDisadvantage>(sqlSfaPostcodeDisadvantage, cancellationToken);

            return sfaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.SfaPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<EfaDisadvantage>>> RetrieveEfaPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlEfaPostcodeDisadvantage = $@"SELECT [Postcode], [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[EFA_PostcodeDisadvantage]";

            var efaDisadvantages = await RetrieveAsync<EfaPostcodeDisadvantage>(sqlEfaPostcodeDisadvantage, cancellationToken);

            return efaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.EfaPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<DasDisadvantage>>> RetrieveDasPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlDasPostcodeDisadvantage = $@"SELECT [Postcode], [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[DAS_PostcodeDisadvantage]";

            var dasDisadvantages = await RetrieveAsync<DasPostcodeDisadvantage>(sqlDasPostcodeDisadvantage, cancellationToken);

            return dasDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.DasPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<ONSData>>> RetrieveOnsData(CancellationToken cancellationToken)
        {
            var sqlOnsData = $@"SELECT [Postcode], [Introduction], [Termination], [LocalAuthority], [Lep1], [Lep2], 
                                                [EffectiveFrom], [EffectiveTo], [Nuts]
                                                FROM [dbo].[ONS_Postcodes]";

            var onsData = await RetrieveAsync<OnsPostcode>(sqlOnsData, cancellationToken);

            return onsData
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.ONSDataToEntity).ToList());
        }

        public async Task<IDictionary<string, List<PostcodeSpecialistResource>>> RetrieveSpecialistResources(CancellationToken cancellationToken)
        {
            var sqlSpecResources = $@"SELECT P.[Postcode], J.[SpecialistResources], J.[EffectiveFrom], J.[EffectiveTo]
                                                FROM [dbo].[PostcodesSpecialistResources]";

            var specResources = await RetrieveAsync<PostcodesSpecialistResource>(sqlSpecResources, cancellationToken);

            return specResources
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.SpecResourcesToEntity).ToList());
        }

        public virtual async Task<IEnumerable<T>> RetrieveAsync<T>(string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.PostcodesConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<T>(commandDefinition);
            }
        }
    }
}
