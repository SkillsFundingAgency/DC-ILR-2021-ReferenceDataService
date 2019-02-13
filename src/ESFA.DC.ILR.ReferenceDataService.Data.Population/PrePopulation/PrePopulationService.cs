using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.PrePopulation
{
    public abstract class PrePopulationService : IPrePopulationService
    {
        // Not sure if needed yet
        // public IEnumerable<FrameworkKey> UniqueFrameworksFromMessage(IMessage message)
        // {
        //     return message?
        //                .Learners?
        //                .Where(l => l.LearningDeliveries != null)
        //                .SelectMany(l => l.LearningDeliveries)
        //                .Where(ld =>
        //                    ld.FworkCodeNullable.HasValue
        //                    && ld.ProgTypeNullable.HasValue
        //                    && ld.PwayCodeNullable.HasValue)
        //                .GroupBy(ld =>
        //                    new
        //                    {
        //                        FworkCode = ld.FworkCodeNullable,
        //                        ProgType = ld.ProgTypeNullable,
        //                        PwayCode = ld.PwayCodeNullable
        //                    })
        //                .Select(g =>
        //                    new FrameworkKey(g.Key.FworkCode.Value, g.Key.ProgType.Value, g.Key.PwayCode.Value))
        //            ?? new List<FrameworkKey>();
        // }

        public IReadOnlyCollection<int> UniqueSTDCodesFromMessage(IMessage message)
        {
            var stdCodes =
                message?
                    .Learners?
                    .Where(l => l.LearningDeliveries != null)
                    .SelectMany(l => l.LearningDeliveries)
                    .Where(ld => ld.StdCodeNullable != null)
                    .Select(ld => ld.StdCodeNullable.Value)
                    .Distinct()
                   ?? new List<int>();

            return stdCodes.ToList();
        }

        public IReadOnlyCollection<int> UniqueEmployerIdsFromMessage(IMessage message)
        {
            var empIds =
                GetLearnerEmpIdsFromFile(message)
                .Union(GetWorkplaceEmpIdsFromFile(message))
                .Distinct() ?? new List<int>();

            return empIds.ToList();
        }

        public IEnumerable<int> GetLearnerEmpIdsFromFile(IMessage message)
        {
            return message?
                    .Learners?
                    .Where(l => l.LearnerEmploymentStatuses != null)
                    .SelectMany(l => l.LearnerEmploymentStatuses)
                    .Where(l => l.EmpIdNullable.HasValue)
                    .Select(l => l.EmpIdNullable.Value)
                ?? new List<int>();
        }

        public IEnumerable<int> GetWorkplaceEmpIdsFromFile(IMessage message)
        {
            return message?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Where(ld => ld.LearningDeliveryWorkPlacements != null)
                       .SelectMany(ld => ld.LearningDeliveryWorkPlacements)
                       .Where(wp => wp.WorkPlaceEmpIdNullable.HasValue)
                       .Select(wp => wp.WorkPlaceEmpIdNullable.Value)
                   ?? new List<int>();
        }

        public IReadOnlyCollection<string> UniquePostcodesFromMessage(IMessage message)
        {
            var postcodes =
                UniqueLearnerPostcodesFromMessage(message)
                .Union(UniqueLearnerPostcodePriorsFromMessage(message))
                .Union(UniqueLearningDeliveryLocationPostcodesFromMessage(message))
                .Distinct() ?? new List<string>();

            return new HashSet<string>(postcodes, StringComparer.OrdinalIgnoreCase);
        }

        public virtual IEnumerable<string> UniqueLearnerPostcodesFromMessage(IMessage message)
        {
            return message?
                        .Learners?
                        .Where(l => l.Postcode != null)
                        .Select(l => l.Postcode)
                        .Distinct()
                    ?? new List<string>();
        }

        public virtual IEnumerable<string> UniqueLearnerPostcodePriorsFromMessage(IMessage message)
        {
            return message?
                       .Learners?
                       .Where(l => l.PostcodePrior != null)
                       .Select(l => l.PostcodePrior)
                       .Distinct()
                   ?? new List<string>();
        }

        public virtual IEnumerable<string> UniqueLearningDeliveryLocationPostcodesFromMessage(IMessage message)
        {
            return message?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Select(ld => ld.DelLocPostCode)
                       .Distinct()
                   ?? new List<string>();
        }

        public IReadOnlyCollection<long> UniqueULNsFromMessage(IMessage message)
        {
            var ulns = new List<long>();

            ulns.AddRange(
                message?
                    .Learners?
                    .Select(l => l.ULN).Distinct() ?? new List<long>());

            ulns.AddRange(
               message?
                   .LearnerDestinationAndProgressions?
                   .Select(l => l.ULN).Distinct() ?? new List<long>());

            return ulns.Distinct().ToList();
        }

        public IReadOnlyCollection<string> UniqueLearnAimRefsFromMessage(IMessage message)
        {
            var learnAimRefs = message?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Where(ld => ld.LearnAimRef != null)
                       .Select(ld => ld.LearnAimRef)
                       .Distinct()
                   ?? new List<string>();

            return new HashSet<string>(learnAimRefs, StringComparer.OrdinalIgnoreCase);
        }

        public virtual IReadOnlyCollection<string> UniqueEpaOrgIdsFromMessage(IMessage message)
        {
            var epaOrgIds =
                message?
                .Learners?
                .SelectMany(l => l.LearningDeliveries
                .Select(ld => ld.EPAOrgID))
                .ToList() ?? new List<string>();

            return new HashSet<string>(epaOrgIds, StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<long> UniqueUKPRNsFromMessage(IMessage message)
        {
            return UniqueLearningProviderUKPRNFromMessage(message)
                .Union(UniqueLearnerPrevUKPRNsFromMessage(message))
                .Union(UniqueLearnerPMUKPRNsFromMessage(message))
                .Union(UniqueLearningDeliveryPartnerUKPRNsFromMessage(message))
                .Distinct()
                .ToList();
        }

        public virtual IEnumerable<long> UniqueLearningProviderUKPRNFromMessage(IMessage message)
        {
            return
                new List<long>
                {
                   message == null ? 0 : message.LearningProviderEntity.UKPRN
                };
        }

        public virtual IEnumerable<long> UniqueLearnerPrevUKPRNsFromMessage(IMessage message)
        {
            return message?
                       .Learners?
                       .Where(l => l.PrevUKPRNNullable != null)
                       .Select(l => (long)l.PrevUKPRNNullable)
                       .Distinct()
                   ?? new List<long>();
        }

        public virtual IEnumerable<long> UniqueLearnerPMUKPRNsFromMessage(IMessage message)
        {
            return message?
                       .Learners?
                       .Where(l => l.PMUKPRNNullable != null)
                       .Select(l => (long)l.PMUKPRNNullable)
                       .Distinct()
                   ?? new List<long>();
        }

        public virtual IEnumerable<long> UniqueLearningDeliveryPartnerUKPRNsFromMessage(IMessage message)
        {
            return message?
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Where(ld => ld.PartnerUKPRNNullable != null)
                       .Select(ld => (long)ld.PartnerUKPRNNullable)
                       .Distinct()
                   ?? new List<long>();
        }
    }
}
