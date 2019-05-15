using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
     public interface IReferenceDataOrchestrationService
     {
         Task Process(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
     }
}
