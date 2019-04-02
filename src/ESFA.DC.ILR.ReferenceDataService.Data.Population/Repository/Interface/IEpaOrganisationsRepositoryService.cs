using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IEpaOrganisationsRepositoryService
    {
        Task<IReadOnlyCollection<EPAOrganisation>> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken);
    }
}
