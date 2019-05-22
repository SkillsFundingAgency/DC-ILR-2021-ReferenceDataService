using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers.Interface
{
    public interface IGzipFileProvider
    {
        Task CompressAndStoreAsync<T>(IReferenceDataContext referenceDataContext, T referenceData, CancellationToken cancellationToken);
    }
}
