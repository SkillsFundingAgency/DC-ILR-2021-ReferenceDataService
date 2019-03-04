using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class EpaOrgIdMapper : IMessageMapper<IReadOnlyCollection<string>>
    {
        public IReadOnlyCollection<string> MapFromMessage(IMessage input)
        {
            var epaOrgIds =
                input?
                .Learners?
                .Where(l => l.LearningDeliveries != null)
                .SelectMany(l => l.LearningDeliveries)
                .Where(ld => ld.EPAOrgID != null)
                .Select(ld => ld.EPAOrgID)
                .ToList() ?? new List<string>();

            return new HashSet<string>(epaOrgIds, StringComparer.OrdinalIgnoreCase);
        }
    }
}
