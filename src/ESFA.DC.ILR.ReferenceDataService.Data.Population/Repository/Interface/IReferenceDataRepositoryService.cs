using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IReferenceDataRepositoryService<TIn, TOut>
    {
        Task<TOut> RetrieveAsync(TIn input, CancellationToken cancellationToken);
    }
}
