using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IAppEarningsHistoryRepositoryService
    {
        Task<IReadOnlyDictionary<long, List<ApprenticeshipEarningsHistory>>> RetrieveAsync(IReadOnlyCollection<long> input, CancellationToken cancellationToken);
    }
}
