using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IRetrievalService<TOut, TMapOut>
    {
        Task<TOut> RetrieveAsync(TMapOut input, CancellationToken cancellationToken);
    }
}
