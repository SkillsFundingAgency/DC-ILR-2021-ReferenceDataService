using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ILR.ReferenceDataService.Providers.Constants;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class ReferenceInputDataMapperService : IReferenceInputDataMapperService
    {
        private readonly IZipArchiveFileService _zipArchiveFileService;
        private readonly IFileService _fileService;
        private readonly ILogger _logger;

        public ReferenceInputDataMapperService(
            IZipArchiveFileService zipArchiveFileService,
            IFileService fileService,
            ILogger logger)
        {
            _zipArchiveFileService = zipArchiveFileService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<DesktopReferenceDataRoot> MapReferenceData(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            var desktopReferenceData = new DesktopReferenceDataRoot();

            var zipFilePath = referenceDataContext.InputReferenceDataFileKey;

            using (var zipFileStream = await _fileService.OpenReadStreamAsync(referenceDataContext.InputReferenceDataFileKey, referenceDataContext.Container, cancellationToken))
            {
                using (var zip = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
                {
                    _logger.LogInfo("Reference Data - Retrieve MetaData");
                    desktopReferenceData.MetaDatas = _zipArchiveFileService.RetrieveModel<MetaData>(zip, DesktopReferenceDataConstants.MetaDataFile);

                    _logger.LogInfo("Reference Data - Retrieve Devolved Postcodes");
                    desktopReferenceData.DevolvedPostocdes = GetDevolvedPostcodes(zip);

                    _logger.LogInfo("Reference Data - Retrieve Employers");
                    desktopReferenceData.Employers = _zipArchiveFileService.RetrieveModels<Employer>(zip, DesktopReferenceDataConstants.EmployersFile);

                    _logger.LogInfo("Reference Data - Retrieve Epa Organisations");
                    desktopReferenceData.EPAOrganisations = _zipArchiveFileService.RetrieveModels<EPAOrganisation>(zip, DesktopReferenceDataConstants.EPAOrganisationsFile);

                    _logger.LogInfo("Reference Data - Retrieve Lars Frameworks");
                    desktopReferenceData.LARSFrameworks = _zipArchiveFileService.RetrieveModels<LARSFrameworkDesktop>(zip, DesktopReferenceDataConstants.LARSFrameworksFile);

                    _logger.LogInfo("Reference Data - Retrieve Lars Framework Aims");
                    desktopReferenceData.LARSFrameworkAims = _zipArchiveFileService.RetrieveModels<LARSFrameworkAimDesktop>(zip, DesktopReferenceDataConstants.LARSFrameworkAimsFile);

                    _logger.LogInfo("Reference Data - Retrieve Lars Learning Deliveries");
                    desktopReferenceData.LARSLearningDeliveries = _zipArchiveFileService.RetrieveModels<LARSLearningDelivery>(zip, DesktopReferenceDataConstants.LARSLearningDeliveriesFile);

                    _logger.LogInfo("Reference Data - Retrieve Lars Standards");
                    desktopReferenceData.LARSStandards = _zipArchiveFileService.RetrieveModels<LARSStandard>(zip, DesktopReferenceDataConstants.LARSStandardsFile);

                    _logger.LogInfo("Reference Data - Retrieve Organisations");
                    desktopReferenceData.Organisations = _zipArchiveFileService.RetrieveModels<Organisation>(zip, DesktopReferenceDataConstants.OrganisationsFile);

                    _logger.LogInfo("Reference Data - Retrieve Postcodes");
                    desktopReferenceData.Postcodes = _zipArchiveFileService.RetrieveModels<Postcode>(zip, DesktopReferenceDataConstants.PostcodesFile);
                }
            }

            return desktopReferenceData;
        }

        public DevolvedPostcodes GetDevolvedPostcodes(ZipArchive zipArchive)
        {
            var sofs = _zipArchiveFileService.RetrieveModels<McaGlaSofLookup>(zipArchive, DesktopReferenceDataConstants.DevolvedMcaGlaSofFile);
            var devolvedPostcodes = _zipArchiveFileService.RetrieveModels<DevolvedPostcode>(zipArchive, DesktopReferenceDataConstants.DevolvedPostcodesFile);

            return new DevolvedPostcodes
            {
                McaGlaSofLookups = sofs.ToList(),
                Postcodes = devolvedPostcodes.ToList(),
            };
        }
    }
}
