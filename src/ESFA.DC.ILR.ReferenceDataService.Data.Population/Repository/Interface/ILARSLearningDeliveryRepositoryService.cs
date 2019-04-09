using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface ILarsLearningDeliveryRepositoryService
    {
        Task<IReadOnlyCollection<LARSLearningDelivery>> RetrieveAsync(IReadOnlyCollection<LARSLearningDeliveryKey> input, CancellationToken cancellationToken);
    }
}
