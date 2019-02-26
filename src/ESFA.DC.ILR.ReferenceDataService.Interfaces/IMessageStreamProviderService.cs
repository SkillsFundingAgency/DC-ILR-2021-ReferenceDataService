using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IMessageStreamProviderService
    {
        Task<Stream> Provide(string fileLocation, CancellationToken cancellationToken);
    }
}
