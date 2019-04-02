using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IPostcodesRepositoryService
    {
        Task<IReadOnlyCollection<Postcode>> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken);
    }
}
