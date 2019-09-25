using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Transactions
{
    public class ValidationMessagesTransaction : IValidationMessagesTransaction
    {
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IIlrReferenceDataRepositoryService _ilrReferenceDataRepositoryService;
        private readonly ILogger _logger;

        public ValidationMessagesTransaction(IReferenceDataOptions referenceDataOptions, IIlrReferenceDataRepositoryService ilrReferenceDataRepositoryService, ILogger logger)
        {
            _referenceDataOptions = referenceDataOptions;
            _ilrReferenceDataRepositoryService = ilrReferenceDataRepositoryService;
            _logger = logger;
        }

        public async Task WriteValidationMessagesAsync(IEnumerable<Rule> rules, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.IlrReferenceDataConnectionString))
            {
                await sqlConnection.OpenAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        _logger.LogInfo("Starting clear of validation rules");
                        await _ilrReferenceDataRepositoryService.ClearValidationRules(sqlConnection, sqlTransaction, cancellationToken);
                        _logger.LogInfo("Finished clear of validation rules");

                        _logger.LogInfo("Starting write of validation rules");
                        await _ilrReferenceDataRepositoryService.WriteValidationRules(rules, sqlConnection, sqlTransaction, cancellationToken);
                        _logger.LogInfo("Finished write of validation rules");

                        cancellationToken.ThrowIfCancellationRequested();
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        _logger.LogInfo($"Validation Messages transaction failed, transaction rolled back - {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
