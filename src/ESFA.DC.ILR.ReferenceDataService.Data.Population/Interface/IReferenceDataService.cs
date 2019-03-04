using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IReferenceDataService<TOut, TMapOut>
    {
        Task<TOut> Retrieve(IMessage message, CancellationToken cancellationToken);
    }
}
