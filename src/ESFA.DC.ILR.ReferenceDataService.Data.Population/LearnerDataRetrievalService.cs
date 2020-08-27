using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Learner;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class LearnerDataRetrievalService : ILearnerDataRetrievalService
    {
        private readonly IUkprnsMapper _ukprnsMapper;
        private readonly ILearnRefNumberMapper _learnRefNumberMapper;
        private readonly ILearnerReferenceDataRepositoryService _learnerReferenceDataRepositoryService;

        public LearnerDataRetrievalService(
        IUkprnsMapper ukprnsMapper,
        ILearnRefNumberMapper learnRefNumberMapper,
        ILearnerReferenceDataRepositoryService learnerReferenceDataRepositoryService)
        {
            _ukprnsMapper = ukprnsMapper;
            _learnRefNumberMapper = learnRefNumberMapper;
            _learnerReferenceDataRepositoryService = learnerReferenceDataRepositoryService;
        }

        public async Task<LearnerReferenceData> RetrieveAsync(IMessage message, CancellationToken cancellationToken)
        {
            var ukprns = _ukprnsMapper.MapUKPRNsFromMessage(message);
            var learnRefNumbers = _learnRefNumberMapper.MapLearnRefNumbersFromMessage(message);

            return new LearnerReferenceData
            {
                Learners = await _learnerReferenceDataRepositoryService.RetrieveLearnerReferenceDataAsync(ukprns, learnRefNumbers, cancellationToken)
            };
        }
    }
}
