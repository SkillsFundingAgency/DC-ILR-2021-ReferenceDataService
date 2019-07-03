using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IReferenceDataRetrievalService<TIn, TOut>
    {
        Task<TOut> RetrieveAsync(TIn input, CancellationToken cancellationToken);
    }
}
