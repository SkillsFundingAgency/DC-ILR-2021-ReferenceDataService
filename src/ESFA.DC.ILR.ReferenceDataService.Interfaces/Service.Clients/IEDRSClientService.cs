using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces.Service.Clients
{
    public interface IEDRSClientService
    {
        Task<IEnumerable<int>> GetInvalidErns(IReadOnlyCollection<int> erns, CancellationToken cancellationToken);
    }
}