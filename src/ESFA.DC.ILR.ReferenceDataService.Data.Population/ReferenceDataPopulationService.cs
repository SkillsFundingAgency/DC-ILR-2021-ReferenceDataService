using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class ReferenceDataPopulationService : IReferenceDataPopulationService
    {
        private readonly IMessageMapperService _messageMapperService;
        private readonly IMetaDataRetrievalService _metaDataRetrievalService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<long>, IReadOnlyCollection<ApprenticeshipEarningsHistory>> _appEarningsHistoryRepositoryService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>> _employersRepositoryService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>> _epaOrganisationsRepositoryService;
        private readonly IReferenceDataRepositoryService<int, IReadOnlyCollection<FcsContractAllocation>> _fcsRepositoryService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> _larsLearningDeliveryRepositoryService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>> _larsStandardRepositoryService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>> _organisationsRepositoryService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>> _postcodesRepositoryService;
        private readonly IReferenceDataRepositoryService<IReadOnlyCollection<long>, IReadOnlyCollection<long>> _ulnRepositoryService;

        public ReferenceDataPopulationService(
            IMessageMapperService messageMapperService,
            IMetaDataRetrievalService metaDataRetrievalService,
            IReferenceDataRepositoryService<IReadOnlyCollection<long>, IReadOnlyCollection<ApprenticeshipEarningsHistory>> appEarningsHistoryRepositoryService,
            IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>> employersRepositoryService,
            IReferenceDataRepositoryService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>> epaOrganisationsRepositoryService,
            IReferenceDataRepositoryService<int, IReadOnlyCollection<FcsContractAllocation>> fcsRepositoryService,
            IReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryRepositoryService,
            IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>> larsStandardRepositoryService,
            IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>> organisationsRepositoryService,
            IReferenceDataRepositoryService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>> postcodesRepositoryService,
            IReferenceDataRepositoryService<IReadOnlyCollection<long>, IReadOnlyCollection<long>> ulnRepositoryService)
        {
            _messageMapperService = messageMapperService;
            _metaDataRetrievalService = metaDataRetrievalService;
            _appEarningsHistoryRepositoryService = appEarningsHistoryRepositoryService;
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
                AppsEarningsHistories = await _appEarningsHistoryRepositoryService.RetrieveAsync(messageData.FM36Ulns, cancellationToken),
                Employers = await _employersRepositoryService.RetrieveAsync(messageData.EmployerIds, cancellationToken),
                EPAOrganisations = await _epaOrganisationsRepositoryService.RetrieveAsync(messageData.EpaOrgIds, cancellationToken),
                FCSContractAllocations = await _fcsRepositoryService.RetrieveAsync(messageData.LearningProviderUKPRN, cancellationToken),
                LARSLearningDeliveries = await _larsLearningDeliveryRepositoryService.RetrieveAsync(messageData.LARSLearningDeliveryKeys, cancellationToken),
                LARSStandards = await _larsStandardRepositoryService.RetrieveAsync(messageData.StandardCodes, cancellationToken),
                Organisations = await _organisationsRepositoryService.RetrieveAsync(messageData.UKPRNs, cancellationToken),
                Postcodes = await _postcodesRepositoryService.RetrieveAsync(messageData.Postcodes, cancellationToken),
                ULNs = await _ulnRepositoryService.RetrieveAsync(messageData.ULNs, cancellationToken),
            };
        }
    }
}
