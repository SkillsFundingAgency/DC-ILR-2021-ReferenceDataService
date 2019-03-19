using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IReferenceDataOutputService
    {
       Task OutputAsync(IReferenceDataContext referenceDataContext, ReferenceDataRoot referenceDataRoot, CancellationToken cancellationToken);
    }
}
