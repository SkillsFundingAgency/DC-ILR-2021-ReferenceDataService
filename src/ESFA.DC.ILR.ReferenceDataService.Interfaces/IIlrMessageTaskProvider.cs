using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IIlrMessageTaskProvider
    {
        Task Provide(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
    }
}
