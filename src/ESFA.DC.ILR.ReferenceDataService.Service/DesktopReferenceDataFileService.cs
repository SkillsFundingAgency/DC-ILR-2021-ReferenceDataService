using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Providers.Constants;
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

            var referenceDataDictionary = new Dictionary<string, object>();

            referenceDataDictionary.Add(DesktopReferenceDataConstants.MetaDataFile, desktopReferenceDataRoot.MetaDatas);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.DevolvedPostcodesFile, desktopReferenceDataRoot.DevolvedPostocdes);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.EmployersFile, desktopReferenceDataRoot.Employers);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.EPAOrganisationsFile, desktopReferenceDataRoot.EPAOrganisations);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSFrameworksFile, desktopReferenceDataRoot.LARSFrameworks);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSFrameworkAimsFile, desktopReferenceDataRoot.LARSFrameworkAims);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSLearningDeliveriesFile, desktopReferenceDataRoot.LARSLearningDeliveries);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSStandardsFile, desktopReferenceDataRoot.LARSStandards);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.OrganisationsFile, desktopReferenceDataRoot.Organisations);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.PostcodesFile, desktopReferenceDataRoot.Postcodes);

            await _zipFileService.SaveCollectionZipAsync(
                fileName,
                referenceDataContext.Container,
                desktopReferenceDataRoot.MetaDatas,
                desktopReferenceDataRoot.DevolvedPostocdes,
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