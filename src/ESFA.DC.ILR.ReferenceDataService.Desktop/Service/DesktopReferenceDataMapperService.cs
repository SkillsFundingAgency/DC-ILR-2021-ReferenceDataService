using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
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
    public class DesktopReferenceDataMapperService : IDesktopReferenceDataMapperService
    {
        private readonly IZipArchiveFileService _zipArchiveFileService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> _larsLearningDeliveryMapperService;
        private readonly IFileService _fileService;
        private readonly ILogger _logger;

        public DesktopReferenceDataMapperService(
            IZipArchiveFileService zipArchiveFileService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryMapperService,
            IFileService fileService,
            ILogger logger)
        {
            _zipArchiveFileService = zipArchiveFileService;
            _larsLearningDeliveryMapperService = larsLearningDeliveryMapperService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<ReferenceDataRoot> MapReferenceData(IReferenceDataContext referenceDataContext, MapperData mapperData, CancellationToken cancellationToken)
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
                    desktopReferenceData.DevolvedPostcodes = GetDevolvedPostcodes(zip, mapperData.Postcodes);

                    _logger.LogInfo("Reference Data - Retrieve Employers");
                    desktopReferenceData.Employers = _zipArchiveFileService.RetrieveModels<Employer>(zip, DesktopReferenceDataConstants.EmployersFile, x => mapperData.EmployerIds.Contains(x.ERN));

                    _logger.LogInfo("Reference Data - Retrieve Epa Organisations");
                    desktopReferenceData.EPAOrganisations = _zipArchiveFileService.RetrieveModels<EPAOrganisation>(zip, DesktopReferenceDataConstants.EPAOrganisationsFile, x => mapperData.EpaOrgIds.Contains(x.ID));

                    _logger.LogInfo("Reference Data - Retrieve Lars Frameworks");
                    desktopReferenceData.LARSFrameworks = _zipArchiveFileService.RetrieveModels<LARSFrameworkDesktop>(zip, DesktopReferenceDataConstants.LARSFrameworksFile);

                    _logger.LogInfo("Reference Data - Retrieve Lars Framework Aims");
                    desktopReferenceData.LARSFrameworkAims = _zipArchiveFileService.RetrieveModels<LARSFrameworkAimDesktop>(zip, DesktopReferenceDataConstants.LARSFrameworkAimsFile);

                    _logger.LogInfo("Reference Data - Retrieve Lars Learning Deliveries");
                    desktopReferenceData.LARSLearningDeliveries = _zipArchiveFileService.RetrieveModels<LARSLearningDelivery>(zip, DesktopReferenceDataConstants.LARSLearningDeliveriesFile, x => mapperData.LARSLearningDeliveryKeys.Select(l => l.LearnAimRef).Contains(x.LearnAimRef, StringComparer.OrdinalIgnoreCase));

                    _logger.LogInfo("Reference Data - Retrieve Lars Standards");
                    desktopReferenceData.LARSStandards = _zipArchiveFileService.RetrieveModels<LARSStandard>(zip, DesktopReferenceDataConstants.LARSStandardsFile, x => mapperData.StandardCodes.Contains(x.StandardCode));

                    _logger.LogInfo("Reference Data - Retrieve Organisations");
                    desktopReferenceData.Organisations = _zipArchiveFileService.RetrieveModels<Organisation>(zip, DesktopReferenceDataConstants.OrganisationsFile, x => mapperData.UKPRNs.Contains(x.UKPRN));

                    _logger.LogInfo("Reference Data - Retrieve Postcodes");
                    desktopReferenceData.Postcodes = _zipArchiveFileService.RetrieveModels<Postcode>(zip, DesktopReferenceDataConstants.PostcodesFile, x => mapperData.Postcodes.Contains(x.PostCode));
                }
            }

            var referenceDataRoot = MapData(desktopReferenceData, mapperData);

            return referenceDataRoot;
        }

        public DevolvedPostcodes GetDevolvedPostcodes(ZipArchive zipArchive, IReadOnlyCollection<string> postcodes)
        {
            var sofs = _zipArchiveFileService.RetrieveModels<McaGlaSofLookup>(zipArchive, DesktopReferenceDataConstants.DevolvedMcaGlaSofFile);
            var devolvedPostcodes = _zipArchiveFileService.RetrieveModels<DevolvedPostcode>(zipArchive, DesktopReferenceDataConstants.DevolvedPostcodesFile, x => postcodes.Contains(x.Postcode));

            return new DevolvedPostcodes
            {
                McaGlaSofLookups = sofs.ToList(),
                Postcodes = devolvedPostcodes.ToList(),
            };
        }

        private ReferenceDataRoot MapData(DesktopReferenceDataRoot desktopReferenceData, MapperData mapperData)
        {
            return new ReferenceDataRoot
            {
                MetaDatas = desktopReferenceData.MetaDatas,
                DevolvedPostcodes = desktopReferenceData.DevolvedPostcodes,
                Employers = desktopReferenceData.Employers,
                EPAOrganisations = desktopReferenceData.EPAOrganisations,
                LARSLearningDeliveries = _larsLearningDeliveryMapperService.Map(mapperData.LARSLearningDeliveryKeys, desktopReferenceData),
                LARSStandards = desktopReferenceData.LARSStandards,
                Organisations = desktopReferenceData.Organisations,
                Postcodes = desktopReferenceData.Postcodes,
            };
        }
    }
}
