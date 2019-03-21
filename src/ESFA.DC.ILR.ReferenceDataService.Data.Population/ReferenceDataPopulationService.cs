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
        private readonly IEmployersRepositoryService _employersRepositoryService;
        private readonly IEpaOrganisationsRepositoryService _epaOrganisationsRepositoryService;
        private readonly IFcsRepositoryService _fcsRepositoryService;
        private readonly ILarsLearningDeliveryRepositoryService _larsLearningDeliveryRepositoryService;
        private readonly ILarsStandardRepositoryService _larsStandardRepositoryService;
        private readonly IOrganisationsRepositoryService _organisationsRepositoryService;
        private readonly IPostcodesRepositoryService _postcodesRepositoryService;
        private readonly IUlnRepositoryService _ulnRepositoryService;

        public ReferenceDataPopulationService(
            IMessageMapperService messageMapperService,
            IMetaDataRetrievalService metaDataRetrievalService,
            IEmployersRepositoryService employersRepositoryService,
            IEpaOrganisationsRepositoryService epaOrganisationsRepositoryService,
            IFcsRepositoryService fcsRepositoryService,
            ILarsLearningDeliveryRepositoryService larsLearningDeliveryRepositoryService,
            ILarsStandardRepositoryService larsStandardRepositoryService,
            IOrganisationsRepositoryService organisationsRepositoryService,
            IPostcodesRepositoryService postcodesRepositoryService,
            IUlnRepositoryService ulnRepositoryService)
        {
            _messageMapperService = messageMapperService;
            _metaDataRetrievalService = metaDataRetrievalService;
            _employersRepositoryService = employersRepositoryService;
            _epaOrganisationsRepositoryService = epaOrganisationsRepositoryService;
            _fcsRepositoryService = fcsRepositoryService;
            _larsLearningDeliveryRepositoryService = larsLearningDeliveryRepositoryService;
            _larsStandardRepositoryService = larsStandardRepositoryService;
            _organisationsRepositoryService = organisationsRepositoryService;
            _postcodesRepositoryService = postcodesRepositoryService;
            _ulnRepositoryService = ulnRepositoryService;
        }

        public async Task<ReferenceDataRoot> PopulateAsync(IMessage message, CancellationToken cancellationToken)
        {
            var messageData = _messageMapperService.MapFromMessage(message);

            return new ReferenceDataRoot
            {
                MetaDatas = await _metaDataRetrievalService.RetrieveAsync(cancellationToken),
                Employers = await _employersRepositoryService.RetrieveAsync(messageData.EmployerIds, cancellationToken),
                EPAOrganisations = await _epaOrganisationsRepositoryService.RetrieveAsync(messageData.EpaOrgIds, cancellationToken),
                FCSContractAllocations = await _fcsRepositoryService.RetrieveAsync(messageData.LearningProviderUKPRN, cancellationToken),
                LARSLearningDeliveries = await _larsLearningDeliveryRepositoryService.RetrieveAsync(messageData.LearnAimRefs, cancellationToken),
                LARSStandards = await _larsStandardRepositoryService.RetrieveAsync(messageData.StandardCodes, cancellationToken),
                Organisations = await _organisationsRepositoryService.RetrieveAsync(messageData.UKPRNs, cancellationToken),
                Postcodes = await _postcodesRepositoryService.RetrieveAsync(messageData.Postcodes, cancellationToken),
                ULNs = await _ulnRepositoryService.RetrieveAsync(messageData.ULNs, cancellationToken)
            };
        }
    }
}
