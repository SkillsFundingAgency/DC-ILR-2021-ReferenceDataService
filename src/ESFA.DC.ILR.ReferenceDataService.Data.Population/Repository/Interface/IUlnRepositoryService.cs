using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.ULNs;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IUlnRepositoryService
    {
        Task<IReadOnlyCollection<ULN>> RetrieveAsync(IReadOnlyCollection<long> input, CancellationToken cancellationToken);
    }
}
