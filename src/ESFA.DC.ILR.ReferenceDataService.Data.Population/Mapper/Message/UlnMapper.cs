using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class UlnMapper : IMessageMapper<IReadOnlyCollection<long>>
    {
        public IReadOnlyCollection<long> MapFromMessage(IMessage input)
        {
            var ulns = new List<long>();

            ulns.AddRange(
                input?
                    .Learners?
                    .Select(l => l.ULN).Distinct() ?? new List<long>());

            ulns.AddRange(
               input?
                   .LearnerDestinationAndProgressions?
                   .Select(l => l.ULN).Distinct() ?? new List<long>());

            return ulns.Distinct().ToList();
        }
    }
}
