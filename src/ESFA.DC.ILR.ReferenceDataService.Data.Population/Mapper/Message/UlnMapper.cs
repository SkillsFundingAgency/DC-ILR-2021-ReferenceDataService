using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class UlnMapper : IUlnMapper
    {
        public IReadOnlyCollection<long> MapUlnsFromMessage(IMessage input)
        {
            var ulns = new List<long>();

            ulns.AddRange(
                input?
                    .Learners?
                    .Select(l => l.ULN).Distinct() ?? Enumerable.Empty<long>());

            ulns.AddRange(
               input?
                   .LearnerDestinationAndProgressions?
                   .Select(l => l.ULN).Distinct() ?? Enumerable.Empty<long>());

            return new HashSet<long>(ulns.Distinct());
        }
    }
}
