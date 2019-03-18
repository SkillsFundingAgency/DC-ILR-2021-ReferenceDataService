using ESFA.DC.ILR.ReferenceDataService.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IReferenceDataOutputService
    {
       Task OutputAsync(IReferenceDataContext referenceDataContext, ReferenceDataRoot referenceDataRoot, CancellationToken cancellationToken);
    }
}
