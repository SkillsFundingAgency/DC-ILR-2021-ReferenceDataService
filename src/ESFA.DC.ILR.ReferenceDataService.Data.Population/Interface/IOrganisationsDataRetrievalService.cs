using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.Organisation;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IOrganisationsDataRetrievalService
    {
        Task<IReadOnlyDictionary<long, Organisation>> RetrieveAsync(IReadOnlyCollection<long> ukprns, CancellationToken cancellationToken);
    }
}
