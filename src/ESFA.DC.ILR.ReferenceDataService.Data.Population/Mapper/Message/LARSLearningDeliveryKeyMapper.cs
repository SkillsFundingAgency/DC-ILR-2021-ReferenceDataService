using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class LARSLearningDeliveryKeyMapper : ILARSLearningDeliveryKeyMapper
    {
        public IReadOnlyCollection<LARSLearningDeliveryKey> MapLARSLearningDeliveryKeysFromMessage(IMessage input)
        {
            return input?

                .Learners?
                .Where(l => l.LearningDeliveries != null)
                .SelectMany(l => l.LearningDeliveries)
                .Where(laf => laf.LearnAimRef != null
                && laf.FworkCodeNullable.HasValue
                && laf.ProgTypeNullable.HasValue
                && laf.PwayCodeNullable.HasValue)
                .Select(ld => new LARSLearningDeliveryKey(ld.LearnAimRef, ld.FworkCodeNullable.Value, ld.ProgTypeNullable.Value, ld.PwayCodeNullable.Value))
                .Distinct().ToList()
            ?? new List<LARSLearningDeliveryKey>();
        }
    }
}
