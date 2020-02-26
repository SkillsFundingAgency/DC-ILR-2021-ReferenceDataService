using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputPersistanceService
    {
        Task PersistModels(IReferenceDataContext referenceDataContext, object efModels, CancellationToken cancellationToken);
    }
}
