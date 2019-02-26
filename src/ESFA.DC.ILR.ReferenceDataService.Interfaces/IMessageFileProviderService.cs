using ESFA.DC.ILR.Model.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IMessageFileProviderService
    {
        Task<IMessage> ProvideAsync(string fileLocation, CancellationToken cancellationToken);
    }
}
