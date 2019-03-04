using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ValidationService.Data.Population.Mapper.Message
{
    public class LearningProviderUkprnMapper : IMessageMapper<int>
    {
        public int MapFromMessage(IMessage input)
        {
            return input == null ? 0 : input.LearningProviderEntity.UKPRN;
        }
    }
}
