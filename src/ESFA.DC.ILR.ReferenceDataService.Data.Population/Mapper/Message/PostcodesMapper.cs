using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class PostcodesMapper : IMessageMapper<IReadOnlyCollection<string>>
    {
        public IReadOnlyCollection<string> MapFromMessage(IMessage input)
        {
            var postcodes =
                UniqueLearnerPostcodesFromMessage(input)
                .Union(UniqueLearnerPostcodePriorsFromMessage(input))
                .Union(UniqueLearningDeliveryLocationPostcodesFromMessage(input))
                .Distinct() ?? new List<string>();

            return new HashSet<string>(postcodes, StringComparer.OrdinalIgnoreCase);
        }

        public virtual IEnumerable<string> UniqueLearnerPostcodesFromMessage(IMessage input)
        {
            return input?
                        .Learners?
                        .Where(l => l.Postcode != null)
                        .Select(l => l.Postcode)
                        .Distinct()
                    ?? new List<string>();
        }

        public virtual IEnumerable<string> UniqueLearnerPostcodePriorsFromMessage(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.PostcodePrior != null)
                       .Select(l => l.PostcodePrior)
                       .Distinct()
                   ?? new List<string>();
        }

        public virtual IEnumerable<string> UniqueLearningDeliveryLocationPostcodesFromMessage(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Select(ld => ld.DelLocPostCode)
                       .Distinct()
                   ?? new List<string>();
        }
    }
}
