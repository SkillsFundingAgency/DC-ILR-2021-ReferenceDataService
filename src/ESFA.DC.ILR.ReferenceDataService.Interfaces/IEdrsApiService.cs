using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IEdrsApiService
    {
        Task<List<int>> ValidateErnsAsync(IMessage message, CancellationToken cancellationToken);
    }
}