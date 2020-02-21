using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class DesktopReferenceDataMapperService : IReferenceDataPopulationService
    {
        private readonly IMessageMapperService _messageMapperService;
        private readonly IDesktopReferenceDataFileRetrievalService _desktopReferenceDataFileRetrievalService;
        private readonly IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> _larsLearningDeliveryMapperService;

        public DesktopReferenceDataMapperService(
            IMessageMapperService messageMapperService,
            IDesktopReferenceDataFileRetrievalService desktopReferenceDataFileRetrievalService,
            IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryMapperService)
        {
            _messageMapperService = messageMapperService;
            _desktopReferenceDataFileRetrievalService = desktopReferenceDataFileRetrievalService;
            _larsLearningDeliveryMapperService = larsLearningDeliveryMapperService;
        }

        public async Task<ReferenceDataRoot> PopulateAsync(IReferenceDataContext referenceDataContext, IMessage message, CancellationToken cancellationToken)
        {
            var mapperData = _messageMapperService.MapFromMessage(message);

            var desktopReferenceData = await _desktopReferenceDataFileRetrievalService.Retrieve(referenceDataContext, mapperData, cancellationToken);

            return new ReferenceDataRoot
            {
                MetaDatas = desktopReferenceData.MetaDatas,
                DevolvedPostocdes = desktopReferenceData.DevolvedPostocdes,
                Employers = desktopReferenceData.Employers,
                EPAOrganisations = desktopReferenceData.EPAOrganisations,
                LARSLearningDeliveries = _larsLearningDeliveryMapperService.Retrieve(mapperData.LARSLearningDeliveryKeys, desktopReferenceData),
                LARSStandards = desktopReferenceData.LARSStandards,
                Organisations = desktopReferenceData.Organisations,
                Postcodes = desktopReferenceData.Postcodes,
            };
        }
    }
}
