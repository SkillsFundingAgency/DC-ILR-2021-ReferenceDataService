using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Learner;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface ILearnerDataRetrievalService
    {
        Task<LearnerReferenceData> RetrieveAsync(IMessage message, CancellationToken cancellationToken);
    }
}
