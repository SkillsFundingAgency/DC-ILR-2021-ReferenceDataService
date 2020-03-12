using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
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

        public async Task<bool> PopulateAsync2(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Truncate existing data");
            await _referenceInputTruncator.TruncateReferenceDataAsync(inputReferenceDataContext, cancellationToken);
            _logger.LogInfo("Finished Truncate existing data");

            using (var sqlConnection = new SqlConnection(inputReferenceDataContext.ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        _logger.LogInfo("Starting population for LarsVersion");
                        await PopulateTopLevelNode<LARSStandard, LARS_LARSStandard>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);
                        _logger.LogInfo("Finished population for LarsVersion");

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

        /*Need to do below process a top level node at a time, to reduce the memory overhead in the application.
         *Processes to be generic driven from the type passed in.
         */
        public async Task<bool> PopulateTopLevelNode<TSource, TTarget>(
            IInputReferenceDataContext inputReferenceDataContext,
            SqlConnection sqlConnection,
            SqlTransaction sqlTransaction,
            CancellationToken cancellationToken)
        {
            var dataFromJson =
                await _desktopReferenceDataRootMapperService.MapReferenceDataByType<TSource>(inputReferenceDataContext, cancellationToken);

            var dataMappedToEF = _referenceInputEFMapper.MapByType<IReadOnlyCollection<TSource>, List<TTarget>>(dataFromJson);

            // Release the resources that came from the json data.
            dataFromJson = null;
            System.GC.Collect();

            _efModelIdentityAssigner.AssignIdsByType<TTarget>(dataMappedToEF);

            // Need to get it into the Db ...
            await _referenceInputPersistenceService.PersistEfModelByTypeAsync(sqlConnection, sqlTransaction, cancellationToken, dataMappedToEF);

            // Release the resources that got mapped into the EF data
            dataMappedToEF = null;
            System.GC.Collect();

            return true;
        }

        public async Task<bool> PopulateAsync(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Reference Data Retrieval from sources");
            var desktopReferenceData = await _desktopReferenceDataRootMapperService.MapReferenceData(inputReferenceDataContext, cancellationToken);
            _logger.LogInfo("Finished Reference Data Retrieval from sources");

            _logger.LogInfo("Starting Reference Data mapping from models to EF models");
            var efReferenceInputDataRoot = _referenceInputEFMapper.Map(desktopReferenceData);
            _logger.LogInfo("Finished Reference Data mapping from models to EF models");

            _logger.LogInfo("Starting assigning ID's to EF models");
            _efModelIdentityAssigner.AssignIds(efReferenceInputDataRoot);
            _logger.LogInfo("Finished assigning ID's to EF models");

            _logger.LogInfo("Starting Truncate existing data");
            await _referenceInputTruncator.TruncateReferenceDataAsync(inputReferenceDataContext, cancellationToken);
            _logger.LogInfo("Finished Truncate existing data");

            _logger.LogInfo("Starting persisting EF Models to Db");
            await _referenceInputPersistenceService.PersistEFModelsAsync(inputReferenceDataContext, efReferenceInputDataRoot, cancellationToken);
            _logger.LogInfo("Finished persisting EF Models to Db");

            return true;
        }
    }
}
