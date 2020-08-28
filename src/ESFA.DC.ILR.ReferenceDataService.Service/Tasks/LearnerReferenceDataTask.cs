using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tasks
{
    public class LearnerReferenceDataTask : ITask
    {
        private readonly bool compressOutput = false;
        private readonly IMessageProvider _messageProvider;
        private readonly ILearnerDataRetrievalService _learnerDataRetrievalService;
        private readonly IFilePersister _filePersister;
        private readonly ILogger _logger;

        public LearnerReferenceDataTask(
            IMessageProvider messageProvider,
            ILearnerDataRetrievalService learnerDataRetrievalService,
            IFilePersister filePersister,
            ILogger logger)
        {
            _messageProvider = messageProvider;
            _learnerDataRetrievalService = learnerDataRetrievalService;
            _filePersister = filePersister;
            _logger = logger;
        }

        public async Task ExecuteAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieving ILR File
                _logger.LogInfo("Starting ILR File Retrieval");
                var message = await _messageProvider.ProvideAsync(referenceDataContext, cancellationToken);
                _logger.LogInfo("Finished retirieving ILR File");

                // get frm reference data and build model
                _logger.LogInfo("Starting Learner Reference Data Population");
                var referenceData = await _learnerDataRetrievalService.RetrieveAsync(message, cancellationToken);
                _logger.LogInfo("Finished Learner Reference Data Population");

                // output model.
                _logger.LogInfo("Starting Learner Reference Data Output");
                await _filePersister.StoreAsync(referenceDataContext.LearnerReferenceDataFileKey, referenceDataContext.Container, referenceData, compressOutput, cancellationToken);
                _logger.LogInfo("Finished Learner Reference Data Output");
            }
            catch (Exception exception)
            {
                _logger.LogError("Learner Reference Data Service Output Exception", exception);
                throw;
            }
        }
    }
}
