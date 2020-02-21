using System;
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
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ILR.ReferenceDataService.Providers.Constants;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class DesktopReferenceDataFileRetrievalService : IDesktopReferenceDataFileRetrievalService
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public DesktopReferenceDataFileRetrievalService(IFileService fileService, IJsonSerializationService jsonSerializationService)
        {
            _fileService = fileService;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<DesktopReferenceDataRoot> Retrieve(IReferenceDataContext referenceDataContext, MapperData mapperData, CancellationToken cancellationToken)
        {
            var referenceData = new DesktopReferenceDataRoot();

            var zipFilePath = referenceDataContext.InputReferenceDataFileKey;

            using (var zipFileStream = await _fileService.OpenReadStreamAsync(referenceDataContext.InputReferenceDataFileKey, referenceDataContext.Container, cancellationToken))
            {
                using (var zip = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
                {
                    referenceData.MetaDatas = RetrieveModel<MetaData>(zip, DesktopReferenceDataConstants.MetaDataFile);
                    referenceData.AppsEarningsHistories = new List<ApprenticeshipEarningsHistory>();
                    referenceData.DevolvedPostocdes = GetDevolvedPostcodes(zip, mapperData.Postcodes);
                    referenceData.Employers = RetrieveModels<Employer>(zip, DesktopReferenceDataConstants.EmployersFile, x => mapperData.EmployerIds.Contains(x.ERN));
                    referenceData.EPAOrganisations = RetrieveModels<EPAOrganisation>(zip, DesktopReferenceDataConstants.EPAOrganisationsFile, x => mapperData.EpaOrgIds.Contains(x.ID, StringComparer.OrdinalIgnoreCase));
                    referenceData.FCSContractAllocations = new List<FcsContractAllocation>();
                    referenceData.LARSFrameworks = RetrieveModels<LARSFrameworkDesktop>(zip, DesktopReferenceDataConstants.LARSFrameworksFile, x => true);
                    referenceData.LARSFrameworkAims = RetrieveModels<LARSFrameworkAimDesktop>(zip, DesktopReferenceDataConstants.LARSFrameworkAimsFile, x => true);
                    referenceData.LARSLearningDeliveries = RetrieveModels<LARSLearningDelivery>(zip, DesktopReferenceDataConstants.LARSLearningDeliveriesFile, x => mapperData.LARSLearningDeliveryKeys.Select(l => l.LearnAimRef).Contains(x.LearnAimRef, StringComparer.OrdinalIgnoreCase));
                    referenceData.LARSStandards = RetrieveModels<LARSStandard>(zip, DesktopReferenceDataConstants.LARSStandardsFile, x => mapperData.StandardCodes.Contains(x.StandardCode));
                    referenceData.Organisations = RetrieveModels<Organisation>(zip, DesktopReferenceDataConstants.OrganisationsFile, x => mapperData.UKPRNs.Contains(x.UKPRN));
                    referenceData.Postcodes = RetrieveModels<Postcode>(zip, DesktopReferenceDataConstants.PostcodesFile, x => mapperData.Postcodes.Contains(x.PostCode, StringComparer.OrdinalIgnoreCase));
                    referenceData.ULNs = new List<long>();
                }
            }

            return referenceData;
        }

        public T RetrieveModel<T>(ZipArchive zipArchive, string fileName)
        {
            T model = (T)Activator.CreateInstance(typeof(T));

            using (var stream = zipArchive.GetEntry(fileName).Open())
            {
                model = _jsonSerializationService.Deserialize<T>(stream);
            }

            return model;
        }

        public IReadOnlyCollection<T> RetrieveModels<T>(ZipArchive zipArchive, string fileName, Func<T, bool> predicate)
        {
            using (var stream = zipArchive.GetEntry(fileName).Open())
            {
                return _jsonSerializationService.DeserializeCollection<T>(stream).Where(predicate).ToList();
            }
        }

        public DevolvedPostcodes GetDevolvedPostcodes(ZipArchive zipArchive, IReadOnlyCollection<string> postcodes)
        {
            var sofs = RetrieveModels<McaGlaSofLookup>(zipArchive, DesktopReferenceDataConstants.DevolvedMcaGlaSofFile, x => true);
            var devolvedPostcodes = RetrieveModels<DevolvedPostcode>(zipArchive, DesktopReferenceDataConstants.DevolvedPostcodesFile, x => postcodes.Contains(x.Postcode, StringComparer.OrdinalIgnoreCase));

            return new DevolvedPostcodes
            {
                McaGlaSofLookups = sofs.ToList(),
                Postcodes = devolvedPostcodes.ToList(),
            };
        }
    }
}
