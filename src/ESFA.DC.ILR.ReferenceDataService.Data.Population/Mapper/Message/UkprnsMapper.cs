using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class UkprnsMapper : IUkprnsMapper
    {
        public IReadOnlyCollection<int> MapUKPRNsFromMessage(IMessage input)
        {
            var ukprns = UniqueLearningProviderUKPRNFromMessage(input)
                .Union(UniqueLearnerPrevUKPRNsFromMessage(input))
                .Union(UniqueLearnerPMUKPRNsFromMessage(input))
                .Union(UniqueLearningDeliveryPartnerUKPRNsFromMessage(input))
                .Distinct();

            return new HashSet<int>(ukprns);
        }

        public virtual IEnumerable<int> UniqueLearningProviderUKPRNFromMessage(IMessage input)
        {
            return
                new List<int>
                {
                   input == null ? 0 : input.LearningProviderEntity.UKPRN,
                };
        }

        public virtual IEnumerable<int> UniqueLearnerPrevUKPRNsFromMessage(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.PrevUKPRNNullable != null)
                       .Select(l => l.PrevUKPRNNullable.Value)
                       .Distinct()
                   ?? Enumerable.Empty<int>();
        }

        public virtual IEnumerable<int> UniqueLearnerPMUKPRNsFromMessage(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.PMUKPRNNullable != null)
                       .Select(l => l.PMUKPRNNullable.Value)
                       .Distinct()
                   ?? Enumerable.Empty<int>();
        }

        public virtual IEnumerable<int> UniqueLearningDeliveryPartnerUKPRNsFromMessage(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Where(ld => ld.PartnerUKPRNNullable != null)
                       .Select(ld => ld.PartnerUKPRNNullable.Value)
                       .Distinct()
                   ?? Enumerable.Empty<int>();
        }
    }
}
