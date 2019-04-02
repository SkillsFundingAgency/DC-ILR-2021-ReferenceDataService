using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface ILarsStandardRepositoryService
    {
        Task<IReadOnlyCollection<LARSStandard>> RetrieveAsync(IReadOnlyCollection<int> input, CancellationToken cancellationToken);
    }
}
