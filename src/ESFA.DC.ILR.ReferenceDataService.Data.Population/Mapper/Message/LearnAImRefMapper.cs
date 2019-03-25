using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class LearnAimRefMapper : ILearnAimRefMapper
    {
        public IReadOnlyCollection<string> MapLearnAimRefsFromMessage(IMessage input)
        {
            var learnAimRefs = input?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Where(ld => ld.LearnAimRef != null)
                       .Select(ld => ld.LearnAimRef)
                       .Distinct()
                   ?? new List<string>();

            return new HashSet<string>(learnAimRefs, StringComparer.OrdinalIgnoreCase);
        }
    }
}
