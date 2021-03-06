﻿using System.Collections.Generic;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
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
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ZipFileService : IZipFileService
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileService _fileService;
        private readonly ILogger _logger;

        public ZipFileService(IJsonSerializationService jsonSerializationService, IFileService fileService, ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task SaveCollectionZipAsync(
            string zipFileName,
            string container,
            MetaData metaDatas,
            DevolvedPostcodes devolvedPostcodes,
            IReadOnlyCollection<Employer> employers,
            IReadOnlyCollection<EPAOrganisation> epaOrganisations,
            IReadOnlyCollection<LARSFrameworkDesktop> larsFrameworks,
            IReadOnlyCollection<LARSFrameworkAimDesktop> larsFrameworkAims,
            IReadOnlyCollection<LARSLearningDelivery> larsLearningDeliveries,
            IReadOnlyCollection<LARSStandard> larsStandards,
            IReadOnlyCollection<Organisation> organisations,
            IReadOnlyCollection<Postcode> postcodes,
            CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Zip File Creation");
            using (var stream = await _fileService.OpenWriteStreamAsync(zipFileName, container, cancellationToken))
            {
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.MetaDataFile, metaDatas);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.DevolvedPostcodesFile, devolvedPostcodes.Postcodes);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.DevolvedMcaGlaSofFile, devolvedPostcodes.McaGlaSofLookups);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.EmployersFile, employers);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.EPAOrganisationsFile, epaOrganisations);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.LARSFrameworksFile, larsFrameworks);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.LARSFrameworkAimsFile, larsFrameworkAims);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.LARSLearningDeliveriesFile, larsLearningDeliveries);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.LARSStandardsFile, larsStandards);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.OrganisationsFile, organisations);
                    AddFileToZip(zipArchive, DesktopReferenceDataConstants.PostcodesFile, postcodes);
                }

                _logger.LogInfo("Finished Zip File Creation");
            }
        }

        private void AddFileToZip<T>(ZipArchive zipArchive, string fileName, T fileContent)
        {
            _logger.LogInfo("Writing " + fileName + " to zip file.");

            var file = zipArchive.CreateEntry(fileName);

            using (var entryStream = file.Open())
            {
                _jsonSerializationService.Serialize(fileContent, entryStream);
            }
        }
    }
}
