using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface ILarsLearningDeliveryRepositoryService
    {
        Task<IReadOnlyDictionary<string, LARSLearningDelivery>> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken);
    }
}
