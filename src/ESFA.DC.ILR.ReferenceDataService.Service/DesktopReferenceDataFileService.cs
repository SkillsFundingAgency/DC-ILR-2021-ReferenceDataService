using System.Collections.Generic;
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
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IZipFileService _zipFileService;
        private readonly ILogger _logger;

        public DesktopReferenceDataFileService(IJsonSerializationService jsonSerializationService, IZipFileService zipFileService, ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _zipFileService = zipFileService;
            _logger = logger;
        }

        public async Task ProcessAync(IReferenceDataContext referenceDataContext, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken)
        {
            var referenceDataDictionary = new Dictionary<string, string>();

            referenceDataDictionary.Add(DesktopReferenceDataConstants.MetaDataFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.MetaDatas));
            referenceDataDictionary.Add(DesktopReferenceDataConstants.EmployersFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.Employers));
            referenceDataDictionary.Add(DesktopReferenceDataConstants.EPAOrganisationsFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.EPAOrganisations));
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSFrameworksFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.LARSFrameworks));
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSLearningDeliveriesFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.LARSLearningDeliveries));
            referenceDataDictionary.Add(DesktopReferenceDataConstants.LARSStandardsFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.LARSStandards));
            referenceDataDictionary.Add(DesktopReferenceDataConstants.OrganisationsFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.Organisations));
            referenceDataDictionary.Add(DesktopReferenceDataConstants.PostcodesFile, _jsonSerializationService.Serialize(desktopReferenceDataRoot.Postcodes));

            await _zipFileService.SaveCollectionZipAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, referenceDataDictionary, cancellationToken);
        }
    }
}
