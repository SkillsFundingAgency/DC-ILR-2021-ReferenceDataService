using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
{
    public class FM36UlnMapper : IFM36UlnMapper
    {
        private const int _fundModel = 36;

        public IReadOnlyCollection<long> MapFM36UlnsFromMessage(IMessage input)
        {
            var ulns = new List<long>();

            ulns.AddRange(
                input?
                .Learners?
                .Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == _fundModel))
                .Select(u => u.ULN)
                ?? new List<long>());

            return ulns.Distinct().ToList();
        }
    }
}
