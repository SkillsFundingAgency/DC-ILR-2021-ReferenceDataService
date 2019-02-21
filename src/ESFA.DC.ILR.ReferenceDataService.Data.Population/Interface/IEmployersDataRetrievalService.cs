using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.Employers;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IEmployersDataRetrievalService
    {
        Task<IReadOnlyCollection<Employers>> RetrieveAsync(IReadOnlyCollection<int> empIds, CancellationToken cancellationToken);
    }
}
