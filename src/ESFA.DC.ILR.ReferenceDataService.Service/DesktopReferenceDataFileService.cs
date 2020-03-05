using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class DesktopReferenceDataFileService : IDesktopReferenceDataFileService
    {
        private readonly IDesktopReferenceDataFileNameService _desktopReferenceDataFileNameService;
        private readonly IZipFileService _zipFileService;
        private readonly ILogger _logger;

        public DesktopReferenceDataFileService(IDesktopReferenceDataFileNameService desktopReferenceDataFileNameService, IZipFileService zipFileService, ILogger logger)
        {
            _desktopReferenceDataFileNameService = desktopReferenceDataFileNameService;
            _zipFileService = zipFileService;
            _logger = logger;
        }

        public async Task ProcessAync(IReferenceDataContext referenceDataContext, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken)
        {
            var fileName = _desktopReferenceDataFileNameService.BuildFileName(referenceDataContext.DesktopReferenceDataStoragePath, referenceDataContext.OutputReferenceDataFileKey);

            await _zipFileService.SaveCollectionZipAsync(
                fileName,
                referenceDataContext.Container,
                desktopReferenceDataRoot.MetaDatas,
                desktopReferenceDataRoot.DevolvedPostcodes,
                desktopReferenceDataRoot.Employers,
                desktopReferenceDataRoot.EPAOrganisations,
                desktopReferenceDataRoot.LARSFrameworks,
                desktopReferenceDataRoot.LARSFrameworkAims,
                desktopReferenceDataRoot.LARSLearningDeliveries,
                desktopReferenceDataRoot.LARSStandards,
                desktopReferenceDataRoot.Organisations,
                desktopReferenceDataRoot.Postcodes,
                cancellationToken);
        }
    }
}