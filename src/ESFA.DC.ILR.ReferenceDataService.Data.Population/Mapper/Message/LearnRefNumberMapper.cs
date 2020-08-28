using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class LearnRefNumberMapper : ILearnRefNumberMapper
    {
        public IReadOnlyCollection<string> MapLearnRefNumbersFromMessage(IMessage input)
        {
            var learnRefNumbers = UniqueLearnRefNumbersFromMessage(input)
                .Union(UniquePrevLearnRefNumbersFromMessage(input))
                .Distinct();

            return new HashSet<string>(learnRefNumbers, StringComparer.OrdinalIgnoreCase);
        }

        public virtual IEnumerable<string> UniqueLearnRefNumbersFromMessage(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.LearnRefNumber != null)
                       .Select(l => l.LearnRefNumber)
                       .Distinct()
                   ?? Enumerable.Empty<string>();
        }

        public virtual IEnumerable<string> UniquePrevLearnRefNumbersFromMessage(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.PrevLearnRefNumber != null)
                       .Select(l => l.PrevLearnRefNumber)
                       .Distinct()
                   ?? Enumerable.Empty<string>();
        }
    }
}
