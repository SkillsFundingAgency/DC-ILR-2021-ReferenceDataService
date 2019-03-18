using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IMessageProvider
    {
        Task<Message> ProvideAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
    }
}
