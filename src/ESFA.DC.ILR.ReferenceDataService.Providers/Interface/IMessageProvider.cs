using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers.Interface
{
    public interface IMessageProvider
    {
        Task<Message> ProvideAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
    }
}
