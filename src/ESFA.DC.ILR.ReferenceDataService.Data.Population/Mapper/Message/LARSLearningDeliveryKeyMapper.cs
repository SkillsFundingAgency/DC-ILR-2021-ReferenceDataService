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
                .Where(laf => laf.LearnAimRef != null)
                .Select(ld => new LARSLearningDeliveryKey(ld.LearnAimRef.ToUpper(), ld.FworkCodeNullable, ld.ProgTypeNullable, ld.PwayCodeNullable))
                .Distinct().ToList()
            ?? new List<LARSLearningDeliveryKey>();
        }
    }
}
