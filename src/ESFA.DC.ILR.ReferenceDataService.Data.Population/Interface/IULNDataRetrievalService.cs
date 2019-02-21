using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IULNDataRetrievalService
    {
        Task<IReadOnlyCollection<long>> RetrieveAsync(IReadOnlyCollection<long> ulns, CancellationToken cancellationToken);
    }
}
