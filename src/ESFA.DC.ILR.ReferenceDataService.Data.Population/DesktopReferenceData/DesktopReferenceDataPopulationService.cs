using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.EAS;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData
{
    public class DesktopReferenceDataPopulationService : IDesktopReferenceDataPopulationService
    {
        private readonly IMetaDataRetrievalService _metaDataRetrievalService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>> _employersRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>> _epaOrganisationsRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>> _larsLearningDeliveryRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>> _larsStandardRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkDesktop>> _larsFrameworkRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>> _organisationsRepositoryService;
        private readonly IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>> _postcodesRepositoryService;

        public DesktopReferenceDataPopulationService(
            IMetaDataRetrievalService metaDataRetrievalService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>> employersRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>> epaOrganisationsRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>> larsStandardRepositoryService,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkDesktop>> larsFrameworkRepositoryService,
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
            var metaDatas = _metaDataRetrievalService.RetrieveAsync(cancellationToken);
            var employers = _employersRepositoryService.RetrieveAsync(cancellationToken);
            var epaOrganisations = _epaOrganisationsRepositoryService.RetrieveAsync(cancellationToken);
            var larsLearningDeliveries = _larsLearningDeliveryRepositoryService.RetrieveAsync(cancellationToken);
            var larsStandards = _larsStandardRepositoryService.RetrieveAsync(cancellationToken);
            var larsFrameworks = _larsFrameworkRepositoryService.RetrieveAsync(cancellationToken);
            var organisations = _organisationsRepositoryService.RetrieveAsync(cancellationToken);
            var postcodes = _postcodesRepositoryService.RetrieveAsync(cancellationToken);

            var taskList = new List<Task>
            {
                metaDatas,
                employers,
                epaOrganisations,
                larsLearningDeliveries,
                larsStandards,
                larsFrameworks,
                organisations,
                postcodes
            };

            await Task.WhenAll(taskList);

            return new DesktopReferenceDataRoot
            {
                MetaDatas = metaDatas.Result,
                AppsEarningsHistories = new List<ApprenticeshipEarningsHistory>(),
                EasFundingLines = new List<EasFundingLine>(),
                Employers = employers.Result,
                EPAOrganisations = epaOrganisations.Result,
                FCSContractAllocations = new List<FcsContractAllocation>(),
                LARSLearningDeliveries = larsLearningDeliveries.Result,
                LARSStandards = larsStandards.Result,
                LARSFrameworks = larsFrameworks.Result,
                Organisations = organisations.Result,
                Postcodes = postcodes.Result,
                ULNs = new List<long>(),
            };
        }
    }
}
