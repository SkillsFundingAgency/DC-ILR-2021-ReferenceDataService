using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Config;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class DesktopReferenceDataFileService : IDesktopReferenceDataFileService
    {
        private readonly IDesktopReferenceDataConfiguration _desktopRefDataConfig;
        private readonly IDesktopReferenceDataFileNameService _desktopReferenceDataFileNameService;
        private readonly IZipFileService _zipFileService;
        private readonly ILogger _logger;

        public DesktopReferenceDataFileService(IDesktopReferenceDataConfiguration desktopRefDataConfig, IDesktopReferenceDataFileNameService desktopReferenceDataFileNameService, IZipFileService zipFileService, ILogger logger)
        {
            _desktopRefDataConfig = desktopRefDataConfig;
            _desktopReferenceDataFileNameService = desktopReferenceDataFileNameService;
            _zipFileService = zipFileService;
            _logger = logger;
        }

        public async Task ProcessAsync(IReferenceDataContext context, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Generating Desktop Reference Data File.");

            var filePath = BuildFilePath(context);
            var fileName = _desktopReferenceDataFileNameService.BuildFileName(filePath, _desktopRefDataConfig.DesktopReferenceDataFilePreFix);
            await _zipFileService.SaveCollectionZipAsync(
                fileName,
                context.Container,
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

        private string BuildFilePath(IReferenceDataContext context)
        {
            return $@"{context.CollectionName}\{context.JobId}";
        }
    }
}