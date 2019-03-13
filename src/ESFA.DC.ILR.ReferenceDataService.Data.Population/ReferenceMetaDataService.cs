using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class ReferenceMetaDataService : IReferenceMetaDataService
    {
        private IMetaDataRetrievalService _retrievalService;

        public ReferenceMetaDataService(IMetaDataRetrievalService retrievalService)
        {
            _retrievalService = retrievalService;
        }

        public Task<MetaData> Retrieve(CancellationToken cancellationToken)
        {
            return _retrievalService.RetrieveAsync(cancellationToken);
        }
    }
}
