using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IEmployersRepositoryService
    {
        Task<IReadOnlyCollection<Employer>> RetrieveAsync(IReadOnlyCollection<int> input, CancellationToken cancellationToken);
    }
}
