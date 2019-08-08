using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Providers.Constants;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class DesktopReferenceDataFileService : IDesktopReferenceDataFileService
    {
        private readonly IZipFileService _zipFileService;
        private readonly ILogger _logger;

        public DesktopReferenceDataFileService(IZipFileService zipFileService, ILogger logger)
        {
            _zipFileService = zipFileService;
            _logger = logger;
        }

        public async Task ProcessAync(IReferenceDataContext referenceDataContext, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken)
        {
            var referenceDataDictionary = new Dictionary<string, object>();

            referenceDataDictionary.Add(DesktopReferenceDataConstants.MetaDataFile, desktopReferenceDataRoot.MetaDatas);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.DevolvedPostcodesFile, desktopReferenceDataRoot.DevolvedPostocdes);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.EmployersFile, desktopReferenceDataRoot.Employers);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.EPAOrganisationsFile, desktopReferenceDataRoot.EPAOrganisations);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSFrameworksFile, desktopReferenceDataRoot.LARSFrameworks);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSLearningDeliveriesFile, desktopReferenceDataRoot.LARSLearningDeliveries);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSStandardsFile, desktopReferenceDataRoot.LARSStandards);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.OrganisationsFile, desktopReferenceDataRoot.Organisations);
            referenceDataDictionary.Add(DesktopReferenceDataConstants.PostcodesFile, desktopReferenceDataRoot.Postcodes);

            await _zipFileService.SaveCollectionZipAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, referenceDataDictionary, cancellationToken);
        }
    }
}
