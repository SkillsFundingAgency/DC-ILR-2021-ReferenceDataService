using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class ReferenceInputDataPopulationService : IReferenceInputDataPopulationService
    {
        private readonly IReferenceInputDataMapperService _desktopReferenceDataRootMapperService;
        private readonly IReferenceInputEFMapper _referenceInputEFMapper;
        private readonly IEFModelIdentityAssigner _efModelIdentityAssigner;
        private readonly IReferenceInputTruncator _referenceInputTruncator;
        private readonly IReferenceInputPersistence _referenceInputPersistenceService;
        private readonly ILogger _logger;

        public ReferenceInputDataPopulationService(
            IReferenceInputDataMapperService desktopReferenceDataRootMapperService,
            IReferenceInputEFMapper referenceInputEFMapper,
            IEFModelIdentityAssigner efModelIdentityAssigner,
            IReferenceInputTruncator referenceInputTruncator,
            IReferenceInputPersistence referenceInputPersistenceService,
            ILogger logger)
        {
            _desktopReferenceDataRootMapperService = desktopReferenceDataRootMapperService;
            _referenceInputEFMapper = referenceInputEFMapper;
            _efModelIdentityAssigner = efModelIdentityAssigner;
            _referenceInputTruncator = referenceInputTruncator;
            _referenceInputPersistenceService = referenceInputPersistenceService;
            _logger = logger;
        }

        public async Task<bool> PopulateAsyncByType(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Truncate existing data");
            _referenceInputTruncator.TruncateReferenceData(inputReferenceDataContext);
            _logger.LogInfo("Finished Truncate existing data");

            using (var sqlConnection = new SqlConnection(inputReferenceDataContext.ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Metadata
                        await PopulateMetaData(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);

                        // Lars data structures
                        await PopulateTopLevelNode<LARSStandard, LARS_LARSStandard>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);
                        await PopulateTopLevelNode<LARSLearningDelivery, LARS_LARSLearningDelivery>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);
                        await PopulateTopLevelNode<LARSFrameworkDesktop, LARS_LARSFrameworkDesktop>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);
                        await PopulateTopLevelNode<LARSFrameworkAimDesktop, LARS_LARSFrameworkAim>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);

                        // Postcode structures
                        await PopulateTopLevelNode<Postcode, Postcodes_Postcode>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);
                        await PopulateTopLevelNode<McaGlaSofLookup, PostcodesDevolution_McaGlaSofLookup>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);
                        await PopulateTopLevelNode<DevolvedPostcode, PostcodesDevolution_Postcode>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);

                        sqlTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        sqlTransaction.Rollback();

                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            return false;
        }

        private async Task<bool> PopulateMetaData(
            IInputReferenceDataContext inputReferenceDataContext,
            SqlConnection sqlConnection,
            SqlTransaction sqlTransaction,
            CancellationToken cancellationToken)
        {
            var metaDataFromJson =
                await _desktopReferenceDataRootMapperService.MapReferenceDataByType<MetaData>(inputReferenceDataContext, cancellationToken);

            var larsVersion = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<LARS_LARSVersion>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<LARS_LARSVersion>(larsVersion);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, larsVersion);

            return false;
        }

        private async Task<bool> PopulateTopLevelNode<TSource, TTarget>(
            IInputReferenceDataContext inputReferenceDataContext,
            SqlConnection sqlConnection,
            SqlTransaction sqlTransaction,
            CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Starting population for {typeof(TTarget).Name}");

            var dataFromJson =
                await _desktopReferenceDataRootMapperService.MapReferenceDataByType<TSource>(inputReferenceDataContext, cancellationToken);

            var dataMappedToEF = _referenceInputEFMapper.MapByType<IReadOnlyCollection<TSource>, List<TTarget>>(dataFromJson);

            // Release the resources that came from the json data.
            dataFromJson = null;
            System.GC.Collect();

            _efModelIdentityAssigner.AssignIdsByType<TTarget>(dataMappedToEF);

            // Need to get it into the Db ...
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, dataMappedToEF);

            // Release the resources that got mapped into the EF data
            dataMappedToEF = null;
            System.GC.Collect();

            _logger.LogInfo($"Finishing population for {typeof(TTarget).Name} RAM usage {System.GC.GetTotalMemory(false)}");
            return true;
        }
    }
}
