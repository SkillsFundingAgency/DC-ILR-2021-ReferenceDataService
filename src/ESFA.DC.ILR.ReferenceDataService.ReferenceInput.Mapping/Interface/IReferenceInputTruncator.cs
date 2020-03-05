using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IReferenceInputTruncator
    {
        Task TruncateReferenceDataAsync(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken);
    }
}
