using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class StandardCodesMapper : IStandardCodesMapper
    {
        public IReadOnlyCollection<int> MapStandardCodesFromMessage(IMessage input)
        {
            var stdCodes =
                input?
                    .Learners?
                    .Where(l => l.LearningDeliveries != null)
                    .SelectMany(l => l.LearningDeliveries)
                    .Where(ld => ld.StdCodeNullable != null)
                    .Select(ld => ld.StdCodeNullable.Value)
                    .Distinct()
                   ?? Enumerable.Empty<int>();

            return new HashSet<int>(stdCodes);
        }
    }
}
