using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop
{
    public class DesktopReferenceDataPopulationService : IDesktopReferenceDataPopulationService
    {
        private readonly IMetaDataRetrievalService _metaDataRetrievalService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>> _employersRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>> _epaOrganisationsRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>> _larsLearningDeliveryRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>> _larsStandardRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFramework>> _larsFrameworkRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>> _organisationsRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>> _postcodesRepositoryService;

        public DesktopReferenceDataPopulationService(
            IMetaDataRetrievalService metaDataRetrievalService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>> employersRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>> epaOrganisationsRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>> larsStandardRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFramework>> larsFrameworkRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>> organisationsRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>> postcodesRepositoryService)
        {
            _metaDataRetrievalService = metaDataRetrievalService;
            _employersRepositoryService = employersRepositoryService;
            _epaOrganisationsRepositoryService = epaOrganisationsRepositoryService;
            _larsLearningDeliveryRepositoryService = larsLearningDeliveryRepositoryService;
            _larsStandardRepositoryService = larsStandardRepositoryService;
            _larsFrameworkRepositoryService = larsFrameworkRepositoryService;
            _organisationsRepositoryService = organisationsRepositoryService;
            _postcodesRepositoryService = postcodesRepositoryService;
        }

        public async Task<DesktopReferenceDataRoot> PopulateAsync(CancellationToken cancellationToken)
        {
            return new DesktopReferenceDataRoot
            {
                DateGenerated = System.DateTime.UtcNow,
                MetaDatas = await _metaDataRetrievalService.RetrieveAsync(cancellationToken),
                AppsEarningsHistories = new List<ApprenticeshipEarningsHistory>(),
                Employers = await _employersRepositoryService.RetrieveAsync(cancellationToken),
                EPAOrganisations = await _epaOrganisationsRepositoryService.RetrieveAsync(cancellationToken),
                FCSContractAllocations = new List<FcsContractAllocation>(),
                LARSLearningDeliveries = await _larsLearningDeliveryRepositoryService.RetrieveAsync(cancellationToken),
                LARSStandards = await _larsStandardRepositoryService.RetrieveAsync(cancellationToken),
                LARSFrameworks = await _larsFrameworkRepositoryService.RetrieveAsync(cancellationToken),
                Organisations = await _organisationsRepositoryService.RetrieveAsync(cancellationToken),
                Postcodes = await _postcodesRepositoryService.RetrieveAsync(cancellationToken),
                ULNs = new List<long>(),
            };
        }
    }
}
