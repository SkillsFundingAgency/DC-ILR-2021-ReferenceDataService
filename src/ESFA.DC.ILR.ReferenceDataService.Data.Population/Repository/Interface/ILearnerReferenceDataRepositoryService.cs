using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.Learner;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface ILearnerReferenceDataRepositoryService
    {
        Task<IReadOnlyCollection<Learner>> RetrieveLearnerReferenceDataAsync(IReadOnlyCollection<int> ukprns, IReadOnlyCollection<string> learnRefNumbers, CancellationToken cancellationToken);
    }
}
