﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.EAS;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.McaContracts;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class ReferenceDataPopulationService : IReferenceDataPopulationService
    {
        private readonly IMessageMapperService _messageMapperService;
        private readonly IMetaDataRetrievalService _metaDataRetrievalService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<ApprenticeshipEarningsHistory>> _appEarningsHistoryRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<string>, DevolvedPostcodes> _devolvedPostcodesRepositoryService;
        private readonly IReferenceDataRetrievalService<int, IReadOnlyCollection<EasFundingLine>> _easRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>> _employersRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>> _epaOrganisationsRepositoryService;
        private readonly IReferenceDataRetrievalService<int, IReadOnlyCollection<FcsContractAllocation>> _fcsRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> _larsLearningDeliveryRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>> _larsStandardRepositoryService;
        private readonly IReferenceDataRetrievalService<int, IReadOnlyCollection<McaDevolvedContract>> _mcaDevolvedContractRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>> _organisationsRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>> _postcodesRepositoryService;
        private readonly IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<long>> _ulnRepositoryService;

        public ReferenceDataPopulationService(
            IMessageMapperService messageMapperService,
            IMetaDataRetrievalService metaDataRetrievalService,
            IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<ApprenticeshipEarningsHistory>> appEarningsHistoryRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<string>, DevolvedPostcodes> devolvedPostcodesRepositoryService,
            IReferenceDataRetrievalService<int, IReadOnlyCollection<EasFundingLine>> easRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>> employersRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>> epaOrganisationsRepositoryService,
            IReferenceDataRetrievalService<int, IReadOnlyCollection<FcsContractAllocation>> fcsRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>> larsStandardRepositoryService,
            IReferenceDataRetrievalService<int, IReadOnlyCollection<McaDevolvedContract>> mcaDevolvedContractRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>> organisationsRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>> postcodesRepositoryService,
            IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<long>> ulnRepositoryService)
        {
            _messageMapperService = messageMapperService;
            _metaDataRetrievalService = metaDataRetrievalService;
            _appEarningsHistoryRepositoryService = appEarningsHistoryRepositoryService;
            _devolvedPostcodesRepositoryService = devolvedPostcodesRepositoryService;
            _easRepositoryService = easRepositoryService;
            _employersRepositoryService = employersRepositoryService;
            _epaOrganisationsRepositoryService = epaOrganisationsRepositoryService;
            _fcsRepositoryService = fcsRepositoryService;
            _larsLearningDeliveryRepositoryService = larsLearningDeliveryRepositoryService;
            _larsStandardRepositoryService = larsStandardRepositoryService;
            _mcaDevolvedContractRepositoryService = mcaDevolvedContractRepositoryService;
            _organisationsRepositoryService = organisationsRepositoryService;
            _postcodesRepositoryService = postcodesRepositoryService;
            _ulnRepositoryService = ulnRepositoryService;
        }

        public async Task<ReferenceDataRoot> PopulateAsync(IReferenceDataContext referenceDataContext, IMessage message, CancellationToken cancellationToken)
        {
            var messageData = _messageMapperService.MapFromMessage(message);

            return new ReferenceDataRoot
            {
                MetaDatas = await _metaDataRetrievalService.RetrieveAsync(messageData.LearningProviderUKPRN, cancellationToken),
                AppsEarningsHistories = await _appEarningsHistoryRepositoryService.RetrieveAsync(messageData.FM36Ulns, cancellationToken),
                DevolvedPostcodes = await _devolvedPostcodesRepositoryService.RetrieveAsync(messageData.Postcodes, cancellationToken),
                EasFundingLines = await _easRepositoryService.RetrieveAsync(messageData.LearningProviderUKPRN, cancellationToken),
                Employers = await _employersRepositoryService.RetrieveAsync(messageData.EmployerIds, cancellationToken),
                EPAOrganisations = await _epaOrganisationsRepositoryService.RetrieveAsync(messageData.EpaOrgIds, cancellationToken),
                FCSContractAllocations = await _fcsRepositoryService.RetrieveAsync(messageData.LearningProviderUKPRN, cancellationToken),
                LARSLearningDeliveries = await _larsLearningDeliveryRepositoryService.RetrieveAsync(messageData.LARSLearningDeliveryKeys, cancellationToken),
                LARSStandards = await _larsStandardRepositoryService.RetrieveAsync(messageData.StandardCodes, cancellationToken),
                McaDevolvedContracts = await _mcaDevolvedContractRepositoryService.RetrieveAsync(messageData.LearningProviderUKPRN, cancellationToken),
                Organisations = await _organisationsRepositoryService.RetrieveAsync(messageData.UKPRNs, cancellationToken),
                Postcodes = await _postcodesRepositoryService.RetrieveAsync(messageData.Postcodes, cancellationToken),
                ULNs = await _ulnRepositoryService.RetrieveAsync(messageData.ULNs, cancellationToken)
            };
        }
    }
}
