using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Message;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model;
using ESFA.DC.Logging.Interfaces;
using Microsoft.SqlServer.Dac;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class ReferenceInputDataPopulationService : IReferenceInputDataPopulationService
    {
        private readonly IMessengerService _messengerService;
        private readonly IReferenceInputDataMapperService _desktopReferenceDataRootMapperService;
        private readonly IReferenceInputEFMapper _referenceInputEFMapper;
        private readonly IEFModelIdentityAssigner _efModelIdentityAssigner;
        private readonly IReferenceInputTruncator _referenceInputTruncator;
        private readonly IReferenceInputPersistence _referenceInputPersistenceService;
        private readonly ILogger _logger;

        public ReferenceInputDataPopulationService(
            IMessengerService messengerService,
            IReferenceInputDataMapperService desktopReferenceDataRootMapperService,
            IReferenceInputEFMapper referenceInputEFMapper,
            IEFModelIdentityAssigner efModelIdentityAssigner,
            IReferenceInputTruncator referenceInputTruncator,
            IReferenceInputPersistence referenceInputPersistenceService,
            ILogger logger)
        {
            _messengerService = messengerService;
            _desktopReferenceDataRootMapperService = desktopReferenceDataRootMapperService;
            _referenceInputEFMapper = referenceInputEFMapper;
            _efModelIdentityAssigner = efModelIdentityAssigner;
            _referenceInputTruncator = referenceInputTruncator;
            _referenceInputPersistenceService = referenceInputPersistenceService;
            _logger = logger;
        }

        public async Task<bool> PopulateAsyncByType(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken)
        {
            const int taskCount = 9;
            var currentTask = 0;

            _messengerService.Send(new TaskProgressMessage("Dacpac starting", currentTask++, taskCount));
            _logger.LogInfo("Starting create from dacpac");
            CreateDatabaseFromDacPack(inputReferenceDataContext.ConnectionString, cancellationToken);
            _logger.LogInfo("Finished create from dacpac");
            _messengerService.Send(new TaskProgressMessage("Dacpac applied", currentTask++, taskCount));

            _logger.LogInfo("Starting Truncate existing data");
            _referenceInputTruncator.TruncateReferenceData(inputReferenceDataContext);
            _logger.LogInfo("Finished Truncate existing data");
            _messengerService.Send(new TaskProgressMessage("Existing data truncated.", currentTask++, taskCount));

            var insertTimeout = inputReferenceDataContext.InsertCommandTimeout;

            using (var sqlConnection = new SqlConnection(inputReferenceDataContext.ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Metadata
                        await PopulateMetaData(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        _messengerService.Send(new TaskProgressMessage("Meta data applied", currentTask++, taskCount));

                        // Lars data structures
                        await PopulateTopLevelNode<LARSStandard, LARS_LARSStandard>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        await PopulateTopLevelNode<LARSLearningDelivery, LARS_LARSLearningDelivery>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        await PopulateTopLevelNode<LARSFrameworkDesktop, LARS_LARSFrameworkDesktop>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        await PopulateTopLevelNode<LARSFrameworkAimDesktop, LARS_LARSFrameworkAim>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        _messengerService.Send(new TaskProgressMessage("Lars data applied", currentTask++, taskCount));

                        // Postcode structures
                        await PopulateTopLevelNode<Postcode, Postcodes_Postcode>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        await PopulateTopLevelNode<McaGlaSofLookup, PostcodesDevolution_McaGlaSofLookup>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        await PopulateTopLevelNode<DevolvedPostcode, PostcodesDevolution_Postcode>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        _messengerService.Send(new TaskProgressMessage("Postcode data applied", currentTask++, taskCount));

                        // Organisations
                        await PopulateTopLevelNode<Organisation, Organisations_Organisation>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        await PopulateTopLevelNode<EPAOrganisation, EPAOrganisations_EPAOrganisation>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        _messengerService.Send(new TaskProgressMessage("Organisations data applied", currentTask++, taskCount));

                        // Employers
                        await PopulateTopLevelNode<Employer, Employers_Employer>(inputReferenceDataContext, sqlConnection, sqlTransaction, insertTimeout, cancellationToken);
                        _messengerService.Send(new TaskProgressMessage("Employers data applied", currentTask++, taskCount));

                        sqlTransaction.Commit();
                        _messengerService.Send(new TaskProgressMessage("Data committed", currentTask++, taskCount));
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

        private DacDeployOptions BuildDacDeployOptions()
        {
            var dacDeployOptions = new DacDeployOptions()
            {
                BlockOnPossibleDataLoss = false,
                CreateNewDatabase = false,
            };

            return dacDeployOptions;
        }

        private string GetDatabaseNameFromInitialCatalog(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }

        private void CreateDatabaseFromDacPack(string connectionString, CancellationToken cancellationToken)
        {
            var dacOptions = BuildDacDeployOptions();

            var databaseName = GetDatabaseNameFromInitialCatalog(connectionString);

            var dacServices = new DacServices(connectionString);

            using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console.Resources.ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Database.dacpac"))
            {
                using (DacPackage dacPackage = DacPackage.Load(stream))
                {
                    dacServices.Deploy(dacPackage, databaseName, true, dacOptions, cancellationToken);
                }
            }
        }

        private async Task<bool> PopulateMetaData(
            IInputReferenceDataContext inputReferenceDataContext,
            SqlConnection sqlConnection,
            SqlTransaction sqlTransaction,
            int bulkCopyTimeout,
            CancellationToken cancellationToken)
        {
            var metaDataFromJson =
                await _desktopReferenceDataRootMapperService.MapReferenceDataByType<MetaData>(inputReferenceDataContext, cancellationToken);

            // Versions
            var referenceDataVersion = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_ReferenceDataVersion>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_ReferenceDataVersion>(referenceDataVersion);

            foreach (var refDataVersion in referenceDataVersion)
            {
                refDataVersion.CampusIdentifierVersion_Id = refDataVersion.CampusIdentifierVersion_?.Id;
                refDataVersion.CoFVersion_Id = refDataVersion.CoFVersion_?.Id;
                refDataVersion.EmployersVersion_Id = refDataVersion.EmployersVersion_?.Id;
                refDataVersion.LarsVersion_Id = refDataVersion.LarsVersion_?.Id;
                refDataVersion.OrganisationsVersion_Id = refDataVersion.OrganisationsVersion_?.Id;
                refDataVersion.PostcodesVersion_Id = refDataVersion.PostcodesVersion_?.Id;
                refDataVersion.DevolvedPostcodesVersion_Id = refDataVersion.DevolvedPostcodesVersion_?.Id;
                refDataVersion.HmppPostcodesVersion_Id = refDataVersion.HmppPostcodesVersion_?.Id;
                refDataVersion.PostcodeFactorsVersion_Id = refDataVersion.PostcodeFactorsVersion_?.Id;
                refDataVersion.EasUploadDateTime_Id = refDataVersion.EasUploadDateTime_?.Id;
            }

            _referenceInputPersistenceService.PersistEfModelByTypeWithoutCollections(sqlConnection, sqlTransaction, inputReferenceDataContext.InsertCommandTimeout, referenceDataVersion);

            var insertTimeout = inputReferenceDataContext.InsertCommandTimeout;

            // Collection Dates
            var censusDates = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_CensusDate>>(metaDataFromJson);
            var returnPeriods = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_ReturnPeriod>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_CensusDate>(censusDates);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_ReturnPeriod>(returnPeriods);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, insertTimeout, censusDates);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, insertTimeout, returnPeriods);

            // Validation Errors / Rules
            var validationErrors = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_ValidationError>>(metaDataFromJson);
            var validationRules = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_ValidationRule>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_ValidationError>(validationErrors);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_ValidationRule>(validationRules);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, insertTimeout, validationErrors);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, insertTimeout, validationRules);

            // Lookups and Subcategories
            var lookups = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_Lookup>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_Lookup>(lookups);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, insertTimeout, lookups);

            // Metadata - Metadata
            var metaData = new MetaData_MetaData
            {
                Id = 1,
                ReferenceDataVersions_Id = referenceDataVersion.First().Id,
                DateGenerated = metaDataFromJson.First().DateGenerated,
            };
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, insertTimeout, new List<MetaData_MetaData> { metaData });

            return false;
        }

        private async Task<bool> PopulateTopLevelNode<TSource, TTarget>(
            IInputReferenceDataContext inputReferenceDataContext,
            SqlConnection sqlConnection,
            SqlTransaction sqlTransaction,
            int bulkCopyTimeout,
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
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, bulkCopyTimeout, dataMappedToEF);

            // Release the resources that got mapped into the EF data
            dataMappedToEF = null;
            System.GC.Collect();

            _logger.LogInfo($"Finishing population for {typeof(TTarget).Name} RAM usage {System.GC.GetTotalMemory(false)}");
            return true;
        }
    }
}
