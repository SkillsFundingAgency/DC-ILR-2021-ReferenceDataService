using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputDataPopulationService
    {
        Task<bool> PopulateAsync(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken);
    }
}
