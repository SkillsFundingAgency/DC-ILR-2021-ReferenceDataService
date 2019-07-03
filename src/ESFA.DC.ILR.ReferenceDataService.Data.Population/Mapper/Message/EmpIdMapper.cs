using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class EmpIdMapper : IEmpIdMapper
    {
        public IReadOnlyCollection<int> MapEmpIdsFromMessage(IMessage input)
        {
            var empIds =
                GetLearnerEmpIdsFromFile(input)
                .Union(GetWorkplaceEmpIdsFromFile(input))
                .Distinct() ?? new List<int>();

            return empIds.ToList();
        }

        private IEnumerable<int> GetLearnerEmpIdsFromFile(IMessage input)
        {
            return input?
                    .Learners?
                    .Where(l => l.LearnerEmploymentStatuses != null)
                    .SelectMany(l => l.LearnerEmploymentStatuses)
                    .Where(l => l.EmpIdNullable.HasValue)
                    .Select(l => l.EmpIdNullable.Value)
                ?? new List<int>();
        }

        private IEnumerable<int> GetWorkplaceEmpIdsFromFile(IMessage input)
        {
            return input?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Where(ld => ld.LearningDeliveryWorkPlacements != null)
                       .SelectMany(ld => ld.LearningDeliveryWorkPlacements)
                       .Where(wp => wp.WorkPlaceEmpIdNullable.HasValue)
                       .Select(wp => wp.WorkPlaceEmpIdNullable.Value)
                   ?? new List<int>();
        }
    }
}
