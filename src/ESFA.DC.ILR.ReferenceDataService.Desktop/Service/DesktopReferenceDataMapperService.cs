using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class DesktopReferenceDataMapperService : IReferenceDataPopulationService
    {
        private readonly IMessageMapperService _messageMapperService;
        private readonly IDesktopReferenceDataFileRetrievalService _desktopReferenceDataFileRetrievalService;
        private readonly IDesktopReferenceMetaDataMapper _metaDataMapper;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<string>, DevolvedPostcodes> _devolvedPostcodesMapperService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>> _employersMapperService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>> _epaOrganisationsMapperService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> _larsLearningDeliveryMapperService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>> _larsStandardMapperService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>> _organisationsMapperService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>> _postcodesMapperService;

        public DesktopReferenceDataMapperService(
            IMessageMapperService messageMapperService,
            IDesktopReferenceDataFileRetrievalService desktopReferenceDataFileRetrievalService,
            IDesktopReferenceMetaDataMapper metaDataMapper,
            IDesktopReferenceDataMapper<IReadOnlyCollection<string>, DevolvedPostcodes> devolvedPostcodesMapperService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>> employersMapperService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>> epaOrganisationsMapperService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryMapperService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>> larsStandardMapperService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>> organisationsMapperService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>> postcodesMapperService)
        {
            _messageMapperService = messageMapperService;
            _desktopReferenceDataFileRetrievalService = desktopReferenceDataFileRetrievalService;
            _metaDataMapper = metaDataMapper;
            _devolvedPostcodesMapperService = devolvedPostcodesMapperService;
            _employersMapperService = employersMapperService;
            _epaOrganisationsMapperService = epaOrganisationsMapperService;
            _larsLearningDeliveryMapperService = larsLearningDeliveryMapperService;
            _larsStandardMapperService = larsStandardMapperService;
            _organisationsMapperService = organisationsMapperService;
            _postcodesMapperService = postcodesMapperService;
        }

        public async Task<ReferenceDataRoot> PopulateAsync(IMessage message, CancellationToken cancellationToken)
        {
            var messageData = _messageMapperService.MapFromMessage(message);

            var desktopReferenceData = _desktopReferenceDataFileRetrievalService.Retrieve();

            return new ReferenceDataRoot
            {
                MetaDatas = _metaDataMapper.Retrieve(desktopReferenceData),
                DevolvedPostocdes = _devolvedPostcodesMapperService.Retrieve(messageData.Postcodes, desktopReferenceData),
                Employers = _employersMapperService.Retrieve(messageData.EmployerIds, desktopReferenceData),
                EPAOrganisations = _epaOrganisationsMapperService.Retrieve(messageData.EpaOrgIds, desktopReferenceData),
                LARSLearningDeliveries = _larsLearningDeliveryMapperService.Retrieve(messageData.LARSLearningDeliveryKeys, desktopReferenceData),
                LARSStandards = _larsStandardMapperService.Retrieve(messageData.StandardCodes, desktopReferenceData),
                Organisations = _organisationsMapperService.Retrieve(messageData.UKPRNs, desktopReferenceData),
                Postcodes = _postcodesMapperService.Retrieve(messageData.Postcodes, desktopReferenceData),
            };
        }
    }
}
