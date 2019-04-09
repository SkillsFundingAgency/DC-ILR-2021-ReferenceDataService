using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface
{
    public interface ILARSLearningDeliveryKeyMapper
    {
        IReadOnlyCollection<LARSLearningDeliveryKey> MapLARSLearningDeliveryKeysFromMessage(IMessage input);
    }
}
