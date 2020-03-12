using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputDataPopulationService
    {
        Task<bool> PopulateAsync2(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken);

        Task<bool> PopulateAsync(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken);
    }
}
