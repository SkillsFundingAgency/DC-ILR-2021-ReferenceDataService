using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers.Interface
{
    public interface IFileProvider
    {
        Task StoreAsync<T>(IReferenceDataContext context, T referenceData, bool compress, CancellationToken cancellationToken);
    }
}
