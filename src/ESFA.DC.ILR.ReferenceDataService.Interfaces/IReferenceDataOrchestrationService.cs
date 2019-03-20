using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
     public interface IReferenceDataOrchestrationService
     {
         Task Retrieve(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
     }
}
