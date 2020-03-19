using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

                        // Organisations
                        await PopulateTopLevelNode<Organisation, Organisations_Organisation>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);
                        await PopulateTopLevelNode<EPAOrganisation, EPAOrganisations_EPAOrganisation>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);

                        // Employers
                        await PopulateTopLevelNode<Employer, Employers_Employer>(inputReferenceDataContext, sqlConnection, sqlTransaction, cancellationToken);

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

            _referenceInputPersistenceService.PersistEfModelByTypeWithoutCollections(sqlConnection, sqlTransaction, referenceDataVersion);

            // Collection Dates
            var censusDates = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_CensusDate>>(metaDataFromJson);
            var returnPeriods = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_ReturnPeriod>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_CensusDate>(censusDates);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_ReturnPeriod>(returnPeriods);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, censusDates);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, returnPeriods);

            // Validation Errors / Rules
            var validationErrors = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_ValidationError>>(metaDataFromJson);
            var validationRules = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_ValidationRule>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_ValidationError>(validationErrors);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_ValidationRule>(validationRules);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, validationErrors);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, validationRules);

            // Lookups and Subcategories
            var lookups = _referenceInputEFMapper.MapByType<IReadOnlyCollection<MetaData>, List<MetaData_Lookup>>(metaDataFromJson);
            _efModelIdentityAssigner.AssignIdsByType<MetaData_Lookup>(lookups);
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, lookups);

            // Metadata - Metadata
            var metaData = new MetaData_MetaData
            {
                Id = 1,
                ReferenceDataVersions_Id = referenceDataVersion.First().Id,
                DateGenerated = metaDataFromJson.First().DateGenerated,
            };
            _referenceInputPersistenceService.PersistEfModelByType(sqlConnection, sqlTransaction, new List<MetaData_MetaData> { metaData });

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
