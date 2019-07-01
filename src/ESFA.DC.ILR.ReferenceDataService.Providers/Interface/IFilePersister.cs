using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Providers.Interface
{
    public interface IFilePersister
    {
        Task StoreAsync<T>(string outputKey, string container, T referenceData, bool compress, CancellationToken cancellationToken);
    }
}
