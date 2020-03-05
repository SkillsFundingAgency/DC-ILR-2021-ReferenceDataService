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
    public class FrmReferenceDataTask : ITask
    {
        private readonly bool compressOutput = false;
        private readonly IFrmDataRetrievalService _frmDataRetrievalService;
        private readonly IFilePersister _filePersister;
        private readonly ILogger _logger;

        public FrmReferenceDataTask(
            IFrmDataRetrievalService frmDataRetrievalService,
            IFilePersister filePersister,
            ILogger logger)
        {
            _frmDataRetrievalService = frmDataRetrievalService;
            _filePersister = filePersister;
            _logger = logger;
        }

        public async Task ExecuteAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            try
            {
                // get frm reference data and build model
                _logger.LogInfo("Starting Frm Reference Data Population");
                var referenceData = await _frmDataRetrievalService.RetrieveAsync(referenceDataContext.Ukprn, cancellationToken);
                _logger.LogInfo("Finished Frm Reference Data Population");

                // output model.
                _logger.LogInfo("Starting Reference Data Output");
                await _filePersister.StoreAsync(referenceDataContext.FrmReferenceDataFileKey, referenceDataContext.Container, referenceData, compressOutput, cancellationToken);
                _logger.LogInfo("Finished Reference Data Output");
            }
            catch (Exception exception)
            {
                _logger.LogError("Reference Data Service Output Exception", exception);
            }
        }
    }
}
