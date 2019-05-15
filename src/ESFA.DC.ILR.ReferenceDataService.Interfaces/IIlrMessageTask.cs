using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
     public interface IIlrMessageTask
     {
         Task Execute(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
     }
}
