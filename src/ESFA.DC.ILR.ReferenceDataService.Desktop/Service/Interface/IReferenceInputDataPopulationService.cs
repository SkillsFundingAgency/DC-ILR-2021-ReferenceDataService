using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputDataPopulationService
    {
        Task<DesktopReferenceDataRoot> PopulateAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
    }
}
