using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
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
        private readonly IJsonSerializationService _jsonSerializationService;

        public DesktopReferenceDataFileRetrievalService(IJsonSerializationService jsonSerializationService)
        {
            _jsonSerializationService = jsonSerializationService;
        }

        public DesktopReferenceDataRoot Retrieve()
        {
            var referenceData = new DesktopReferenceDataRoot();
            var zipFilePath = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault();

            using (var zipFileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(zipFilePath))
            {
                using (var zip = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
                {
                    referenceData.MetaDatas = RetrieveModel<MetaData>(zip, DesktopReferenceDataConstants.MetaDataFile);
                    referenceData.AppsEarningsHistories = new List<ApprenticeshipEarningsHistory>();
                    referenceData.DevolvedPostocdes = RetrieveModel<DevolvedPostcodes>(zip, DesktopReferenceDataConstants.DevolvedPostcodesFile);
                    referenceData.Employers = RetrieveModel<List<Employer>>(zip, DesktopReferenceDataConstants.EmployersFile);
                    referenceData.EPAOrganisations = RetrieveModel<List<EPAOrganisation>>(zip, DesktopReferenceDataConstants.EPAOrganisationsFile);
                    referenceData.FCSContractAllocations = new List<FcsContractAllocation>();
                    referenceData.LARSFrameworks = RetrieveModel<List<LARSFrameworkDesktop>>(zip, DesktopReferenceDataConstants.LARSFrameworksFile);
                    referenceData.LARSFrameworkAims = RetrieveModel<List<LARSFrameworkAimDesktop>>(zip, DesktopReferenceDataConstants.LARSFrameworkAimsFile);
                    referenceData.LARSLearningDeliveries = RetrieveModel<List<LARSLearningDelivery>>(zip, DesktopReferenceDataConstants.LARSLearningDeliveriesFile);
                    referenceData.LARSStandards = RetrieveModel<List<LARSStandard>>(zip, DesktopReferenceDataConstants.LARSStandardsFile);
                    referenceData.Organisations = RetrieveModel<List<Organisation>>(zip, DesktopReferenceDataConstants.OrganisationsFile);
                    referenceData.Postcodes = RetrieveModel<List<Postcode>>(zip, DesktopReferenceDataConstants.PostcodesFile);
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
    }
}
