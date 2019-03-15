using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IStronglyTypedKeyValuePersistenceService
    {
        Task SaveAsync<T>(string key, T value, CancellationToken cancellationToken);

        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken);
    }
}
