using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.CsvModels;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Mappers;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tasks
{
    public class ValidationMessagesTask : ITask
    {
        private readonly ILogger _logger;
        private readonly ICsvRetrievalService _csvRetrievalService;
        private readonly IValidationMessagesTransaction _validationMessagesTransaction;

        public ValidationMessagesTask(ILogger logger, ICsvRetrievalService csvRetrievalService, IValidationMessagesTransaction validationMessagesTransaction)
        {
            _logger = logger;
            _csvRetrievalService = csvRetrievalService;
            _validationMessagesTransaction = validationMessagesTransaction;
        }

        public async Task ExecuteAsync(IReferenceDataContext context, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Retrieval of Validation Messages Csv data");

            var csvRecords = await _csvRetrievalService.RetrieveCsvData<ValidationMessagesModel, ValidationMessagesMapper>(context.ValidationMessagesFileReference, context.Container, cancellationToken);

            if (csvRecords.Any())
            {
                _logger.LogInfo("Finished Retrieval of Validation Messages Csv data");
                await _validationMessagesTransaction.WriteValidationMessagesAsync(BuildRules(csvRecords), cancellationToken);
            }
            else
            {
                _logger.LogInfo("Validation Messages Csv is empty - exiting validation messages task");
            }
        }

        private IEnumerable<Rule> BuildRules(IEnumerable<ValidationMessagesModel> models)
        {
            return models.Select(model => new Rule
            {
                Rulename = model.RuleName,
                Message = model.ErrorMessage,
                Severity = model.Severity,
                Online = model.EnabledSLD,
                Desktop = model.EnabledDesktop,
            });
        }
    }
}
