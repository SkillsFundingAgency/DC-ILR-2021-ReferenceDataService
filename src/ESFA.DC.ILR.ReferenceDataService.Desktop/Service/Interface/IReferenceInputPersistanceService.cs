using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputPersistanceService
    {
        Task PersistModels<T>(IReferenceDataContext referenceDataContext, T efModels, CancellationToken cancellationToken);
    }
}
