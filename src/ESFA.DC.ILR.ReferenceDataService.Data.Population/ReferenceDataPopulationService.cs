using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.ULNs;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class ReferenceDataPopulationService : IReferenceDataPopulationService
    {
        private readonly IReferenceMetaDataService _metaDataReferenceService;

        private readonly IReferenceDataService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>> _employersReferenceDataService;
        private readonly IReferenceDataService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>> _epaOrgReferenceDataService;
        private readonly IReferenceDataService<IReadOnlyDictionary<string, FcsContractAllocation>, int> _fcsReferenceDataService;
        private readonly IReferenceDataService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>> _larsLearningDeliveryReferenceDataService;
        private readonly IReferenceDataService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>> _larsStandardReferenceDataService;
        private readonly IReferenceDataService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>> _orgReferenceDataService;
        private readonly IReferenceDataService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>> _postcodeReferenceDataService;
        private readonly IReferenceDataService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>> _ulnReferenceDataService;

        public ReferenceDataPopulationService(
            IReferenceMetaDataService metaDataReferenceService,
            IReferenceDataService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>> employersReferenceDataService,
            IReferenceDataService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>> epaOrgReferenceDataService,
            IReferenceDataService<IReadOnlyDictionary<string, FcsContractAllocation>, int> fcsReferenceDataService,
            IReferenceDataService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>> larsLearningDeliveryReferenceDataService,
            IReferenceDataService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>> larsStandardReferenceDataService,
            IReferenceDataService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>> orgReferenceDataService,
            IReferenceDataService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>> postcodeReferenceDataService,
            IReferenceDataService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>> ulnReferenceDataService)
        {
            _metaDataReferenceService = metaDataReferenceService;
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
            return new ReferenceDataRoot
            {
                MetaDatas = await _metaDataReferenceService.Retrieve(cancellationToken),
                Employers = await _employersReferenceDataService.Retrieve(message, cancellationToken),
                EPAOrganisations = await _epaOrgReferenceDataService.Retrieve(message, cancellationToken),
                FCSContractAllocations = await _fcsReferenceDataService.Retrieve(message, cancellationToken),
                LARSLearningDeliveries = await _larsLearningDeliveryReferenceDataService.Retrieve(message, cancellationToken),
                LARSStandards = await _larsStandardReferenceDataService.Retrieve(message, cancellationToken),
                Organisations = await _orgReferenceDataService.Retrieve(message, cancellationToken),
                Postcodes = await _postcodeReferenceDataService.Retrieve(message, cancellationToken),
                ULNs = await _ulnReferenceDataService.Retrieve(message, cancellationToken)
            };
        }
    }
}
