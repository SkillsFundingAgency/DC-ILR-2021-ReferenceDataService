using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class LarsLearningDeliveryReferenceDataMapper : IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>
    {
        public IReadOnlyCollection<LARSLearningDelivery> Retrieve(IReadOnlyCollection<LARSLearningDeliveryKey> input, DesktopReferenceDataRoot referenceData)
        {
            var larsFrameworks = new List<LARSFrameworkKey>();

            var larsLearningDeliveries = referenceData.LARSLearningDeliveries
                .Where(l => input.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
                .ToList();

            var larsFrameworkAims = referenceData.LARSFrameworkAims
                 .Where(l => input.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
                .ToList();

            var larsFrameworkAimsDictionary = referenceData.LARSFrameworkAims
               .GroupBy(ld => new LARSLearningDeliveryKey(ld.LearnAimRef.ToUpper(), ld.FworkCode, ld.ProgType, ld.PwayCode))
               .ToDictionary(
                k => k.Key,
                v => v.Select(fa => new LARSFrameworkAim
                {
                    LearnAimRef = fa.LearnAimRef.ToUpper(),
                    FrameworkComponentType = fa.FrameworkComponentType,
                    EffectiveFrom = fa.EffectiveFrom,
                    EffectiveTo = fa.EffectiveTo,
                }).FirstOrDefault());

            foreach (var key in input)
            {
                var framework = referenceData.LARSFrameworks
                    .Where(lf =>
                        lf.FworkCode == key.FworkCode
                    && lf.ProgType == key.ProgType
                    && lf.PwayCode == key.PwayCode)
                    .Select(l => new LARSFramework
                    {
                        FworkCode = l.FworkCode,
                        PwayCode = l.PwayCode,
                        ProgType = l.ProgType,
                        EffectiveFromNullable = l.EffectiveFromNullable,
                        EffectiveTo = l.EffectiveTo,
                        LARSFrameworkApprenticeshipFundings = l.LARSFrameworkApprenticeshipFundings,
                        LARSFrameworkCommonComponents = l.LARSFrameworkCommonComponents,
                        LARSFrameworkAim = larsFrameworkAimsDictionary.TryGetValue(key, out var frameworkAims) ? frameworkAims : null,
                    }).FirstOrDefault();

                if (framework != null)
                {
                    larsFrameworks.Add(new LARSFrameworkKey(key.LearnAimRef, framework));
                }
            }

            var frameworkDictionary = larsFrameworks.GroupBy(l => l.LearnAimRef).ToDictionary(k => k.Key, v => v.Select(l => l.LARSFramework).ToList());

            foreach (var learningDelivery in larsLearningDeliveries)
            {
                frameworkDictionary.TryGetValue(learningDelivery.LearnAimRef, out var frameworks);

                learningDelivery.LARSFrameworks = frameworks;
            }

            return larsLearningDeliveries;
        }
    }
}
