using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Learner;
using ESFA.DC.ILR1920.DataStore.EF.Valid.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class LearnerReferenceDataRepositoryService : ILearnerReferenceDataRepositoryService
    {
        private readonly IDbContextFactory<IILR1920_DataStoreEntitiesValid> _ilrContextFactory;
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IJsonSerializationService _jsonSerializationService;

        private readonly string sql = @"SELECT  
                                           L.[UKPRN]
                                          ,L.[LearnRefNumber]
                                          ,L.[PrevLearnRefNumber]
                                          ,L.[PrevUKPRN]
                                          ,L.[PMUKPRN]
                                          ,L.[ULN]
	                                      ,LD.[LearnAimRef]
	                                      ,LD.[ProgType] AS ProgTypeNullable
	                                      ,LD.[PwayCode] AS PwayCodeNullable
	                                      ,LD.[FworkCode] AS FworkCodeNullable
	                                      ,LD.[StdCode] AS StdCodeNullable
	                                      ,LD.[FundModel] AS FundModel
	                                      ,LD.[LearnStartDate]
	                                      ,LD.[LearnActEndDate]
                                      FROM [Valid].[Learner] L
                                      INNER JOIN [Valid].[LearningDelivery] LD
                                      ON LD.UKPRN = L.UKPRN
                                      AND LD.LearnRefNumber = L.LearnRefNumber
                                      INNER JOIN OPENJSON(@ukprnsSerialized) WITH(UKPRN int '$') JUkprn
                                      ON JUkprn.UKPRN = L.UKPRN
                                      INNER JOIN OPENJSON(@learnRefNumbersSerialized) WITH(LearnRefNumber nvarchar(12) '$') JLearner
                                      ON JLearner.LearnRefNumber = L.LearnRefNumber";

        public LearnerReferenceDataRepositoryService(
            IDbContextFactory<IILR1920_DataStoreEntitiesValid> ilrContextFactory,
            IReferenceDataOptions referenceDataOptions,
            IJsonSerializationService jsonSerializationService)
        {
            _ilrContextFactory = ilrContextFactory;
            _referenceDataOptions = referenceDataOptions;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<IReadOnlyCollection<Learner>> RetrieveLearnerReferenceDataAsync(IReadOnlyCollection<int> ukprns, IReadOnlyCollection<string> learnRefNumbers, CancellationToken cancellationToken)
        {
            var ukprnsSerialized = _jsonSerializationService.Serialize(ukprns);
            var learnRefNumbersSerialized = _jsonSerializationService.Serialize(learnRefNumbers);

            using (var context = _ilrContextFactory.Create())
            {
                var learners = await RetrieveAsync(ukprnsSerialized, learnRefNumbersSerialized, sql, cancellationToken);

                return learners.ToList();
            }
        }

        public virtual async Task<IEnumerable<Learner>> RetrieveAsync(string ukprnsSerialized, string learnRefNumbersSerialized, string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.Ilr1920ConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, new { ukprnsSerialized, learnRefNumbersSerialized }, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<Learner>(commandDefinition);
            }
        }
    }
}
