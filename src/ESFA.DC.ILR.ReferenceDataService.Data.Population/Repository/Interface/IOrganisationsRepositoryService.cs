using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IOrganisationsRepositoryService
    {
        Task<IReadOnlyDictionary<int, Organisation>> RetrieveAsync(IReadOnlyCollection<int> input, CancellationToken cancellationToken);
    }
}
