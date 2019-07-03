using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface
{
    public interface ILARSLearningDeliveryKeyMapper
    {
        IReadOnlyCollection<LARSLearningDeliveryKey> MapLARSLearningDeliveryKeysFromMessage(IMessage input);
    }
}
