// using System.Collections.Generic;
// using System.Linq;
// using ESFA.DC.ILR.Model.Interface;
// using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
//
// namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message
// {
//     public class FrameworksMapper : IMessageMapper<IReadOnlyCollection<FrameworkKey>>
//     {
//         // Not sure if needed yet
//          public IReadOnlyCollection<FrameworkKey> MapFromMessage(IMessage input)
//         {
//             return input?
//                        .Learners?
//                        .Where(l => l.LearningDeliveries != null)
//                        .SelectMany(l => l.LearningDeliveries)
//                        .Where(ld =>
//                            ld.FworkCodeNullable.HasValue
//                            && ld.ProgTypeNullable.HasValue
//                            && ld.PwayCodeNullable.HasValue)
//                        .GroupBy(ld =>
//                            new
//                            {
//                                FworkCode = ld.FworkCodeNullable,
//                                ProgType = ld.ProgTypeNullable,
//                                PwayCode = ld.PwayCodeNullable
//                            })
//                        .Select(g =>
//                            new FrameworkKey(g.Key.FworkCode.Value, g.Key.ProgType.Value, g.Key.PwayCode.Value))
//                    ?? new List<FrameworkKey>();
//         }
//     }
// }
