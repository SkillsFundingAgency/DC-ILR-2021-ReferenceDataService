using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class StdCodeMapper : IMessageMapper<IReadOnlyCollection<int>>
    {
        public IReadOnlyCollection<int> MapFromMessage(IMessage input)
        {
            var stdCodes =
                input?
                    .Learners?
                    .Where(l => l.LearningDeliveries != null)
                    .SelectMany(l => l.LearningDeliveries)
                    .Where(ld => ld.StdCodeNullable != null)
                    .Select(ld => ld.StdCodeNullable.Value)
                    .Distinct()
                   ?? new List<int>();

            return stdCodes.ToList();
        }
    }
}
