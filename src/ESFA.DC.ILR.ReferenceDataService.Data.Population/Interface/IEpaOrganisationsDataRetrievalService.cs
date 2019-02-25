using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.EPAOrganisation;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IEpaOrganisationsDataRetrievalService
    {
        Task<IReadOnlyDictionary<string, List<EPAOrganisation>>> RetrieveAsync(IReadOnlyCollection<string> eepaOrgIds, CancellationToken cancellationToken);
    }
}
