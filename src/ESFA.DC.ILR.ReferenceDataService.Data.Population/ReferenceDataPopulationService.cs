using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class ReferenceDataPopulationService : IReferenceDataPopulationService
    {
        private readonly IMessageMapperService _messageMapperService;
        private readonly IMetaDataRetrievalService _metaDataRetrievalService;
        private readonly IEmployersRepositoryService _employersReferenceDataService;
        private readonly IEpaOrganisationsRepositoryService _epaOrgReferenceDataService;
        private readonly IFcsRepositoryService _fcsReferenceDataService;
        private readonly ILarsLearningDeliveryRepositoryService _larsLearningDeliveryReferenceDataService;
        private readonly ILarsStandardRepositoryService _larsStandardReferenceDataService;
        private readonly IOrganisationsRepositoryService _orgReferenceDataService;
        private readonly IPostcodesRepositoryService _postcodeReferenceDataService;
        private readonly IUlnRepositoryService _ulnReferenceDataService;

        public ReferenceDataPopulationService(
            IMessageMapperService messageMapperService,
            IMetaDataRetrievalService metaDataRetrievalService,
            IEmployersRepositoryService employersReferenceDataService,
            IEpaOrganisationsRepositoryService epaOrgReferenceDataService,
            IFcsRepositoryService fcsReferenceDataService,
            ILarsLearningDeliveryRepositoryService larsLearningDeliveryReferenceDataService,
            ILarsStandardRepositoryService larsStandardReferenceDataService,
            IOrganisationsRepositoryService orgReferenceDataService,
            IPostcodesRepositoryService postcodeReferenceDataService,
            IUlnRepositoryService ulnReferenceDataService)
        {
            _messageMapperService = messageMapperService;
            _metaDataRetrievalService = metaDataRetrievalService;
            _employersReferenceDataService = employersReferenceDataService;
            _epaOrgReferenceDataService = epaOrgReferenceDataService;
            _fcsReferenceDataService = fcsReferenceDataService;
            _larsLearningDeliveryReferenceDataService = larsLearningDeliveryReferenceDataService;
            _larsStandardReferenceDataService = larsStandardReferenceDataService;
            _orgReferenceDataService = orgReferenceDataService;
            _postcodeReferenceDataService = postcodeReferenceDataService;
            _ulnReferenceDataService = ulnReferenceDataService;
        }

        public async Task<ReferenceDataRoot> PopulateAsync(IMessage message, CancellationToken cancellationToken)
        {
            var messageData = _messageMapperService.MapFromMessage(message);

            return new ReferenceDataRoot
            {
                MetaDatas = await _metaDataRetrievalService.RetrieveAsync(cancellationToken),
                Employers = await _employersReferenceDataService.RetrieveAsync(messageData.EmployerIds, cancellationToken),
                EPAOrganisations = await _epaOrgReferenceDataService.RetrieveAsync(messageData.EpaOrgIds, cancellationToken),
                FCSContractAllocations = await _fcsReferenceDataService.RetrieveAsync(messageData.LearningProviderUKPRN, cancellationToken),
                LARSLearningDeliveries = await _larsLearningDeliveryReferenceDataService.RetrieveAsync(messageData.LearnAimRefs, cancellationToken),
                LARSStandards = await _larsStandardReferenceDataService.RetrieveAsync(messageData.StandardCodes, cancellationToken),
                Organisations = await _orgReferenceDataService.RetrieveAsync(messageData.UKPRNs, cancellationToken),
                Postcodes = await _postcodeReferenceDataService.RetrieveAsync(messageData.Postcodes, cancellationToken),
                ULNs = await _ulnReferenceDataService.RetrieveAsync(messageData.ULNs, cancellationToken)
            };
        }
    }
}
