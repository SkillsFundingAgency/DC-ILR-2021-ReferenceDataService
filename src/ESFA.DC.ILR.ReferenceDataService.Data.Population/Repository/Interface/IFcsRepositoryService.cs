using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IFcsRepositoryService
    {
        Task<IReadOnlyDictionary<string, FcsContractAllocation>> RetrieveAsync(int input, CancellationToken cancellationToken);
    }
}
