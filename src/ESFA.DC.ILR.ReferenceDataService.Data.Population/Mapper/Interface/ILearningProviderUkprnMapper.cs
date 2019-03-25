using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface
{
    public interface ILearningProviderUkprnMapper
    {
        int MapLearningProviderUKPRNFromMessage(IMessage input);
    }
}
