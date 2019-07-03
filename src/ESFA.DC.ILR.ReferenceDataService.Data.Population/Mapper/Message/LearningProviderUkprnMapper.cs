using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class LearningProviderUkprnMapper : ILearningProviderUkprnMapper
    {
        public int MapLearningProviderUKPRNFromMessage(IMessage input)
        {
            return input == null ? 0 : input.LearningProviderEntity.UKPRN;
        }
    }
}
