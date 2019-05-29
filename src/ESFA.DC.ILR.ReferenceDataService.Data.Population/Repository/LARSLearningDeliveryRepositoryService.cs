using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class LarsLearningDeliveryRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>
    {
        private readonly ILARSContext _larsContext;

        public LarsLearningDeliveryRepositoryService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyCollection<LARSLearningDelivery>> RetrieveAsync(IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, CancellationToken cancellationToken)
        {
            var larsFrameworks = new List<LARSFrameworkKey>();

            var learningDeliveries = await _larsContext.LARS_LearningDeliveries
                .Where(l => inputKeys.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef))
                .Select(
                    ld => new LARSLearningDelivery
                    {
                        LearnAimRef = ld.LearnAimRef,
                        LearnAimRefTitle = ld.LearnAimRefTitle,
                        LearnAimRefType = ld.LearnAimRefType,
                        LearningDeliveryGenre = ld.LearningDeliveryGenre,
                        LearnDirectClassSystemCode1 = ld.LearnDirectClassSystemCode1,
                        LearnDirectClassSystemCode2 = ld.LearnDirectClassSystemCode2,
                        LearnDirectClassSystemCode3 = ld.LearnDirectClassSystemCode3,
                        AwardOrgCode = ld.AwardOrgCode,
                        EFACOFType = ld.Efacoftype,
                        EffectiveFrom = ld.EffectiveFrom,
                        EffectiveTo = ld.EffectiveTo,
                        EnglandFEHEStatus = ld.EnglandFehestatus,
                        EnglPrscID = ld.EnglPrscId,
                        FrameworkCommonComponent = ld.FrameworkCommonComponent,
                        NotionalNVQLevel = ld.NotionalNvqlevel,
                        NotionalNVQLevelv2 = ld.NotionalNvqlevelv2,
                        RegulatedCreditValue = ld.RegulatedCreditValue,
                        SectorSubjectAreaTier1 = ld.SectorSubjectAreaTier1,
                        SectorSubjectAreaTier2 = ld.SectorSubjectAreaTier2,
                        LARSAnnualValues = ld.LarsAnnualValues.Select(la =>
                        new LARSAnnualValue
                        {
                            BasicSkills = la.BasicSkills,
                            BasicSkillsType = la.BasicSkillsType,
                            EffectiveFrom = la.EffectiveFrom,
                            EffectiveTo = la.EffectiveTo,
                            FullLevel2EntitlementCategory = la.FullLevel2EntitlementCategory,
                            FullLevel3EntitlementCategory = la.FullLevel3EntitlementCategory,
                            FullLevel3Percent = la.FullLevel3Percent,
                        }).ToList(),
                        LARSCareerLearningPilots = ld.LarsCareerLearningPilots.Select(lc =>
                        new LARSCareerLearningPilot
                        {
                            AreaCode = lc.AreaCode,
                            EffectiveFrom = lc.EffectiveFrom,
                            EffectiveTo = lc.EffectiveTo,
                            SubsidyRate = lc.SubsidyRate,
                        }).ToList(),
                        LARSLearningDeliveryCategories = ld.LarsLearningDeliveryCategories.Select(ldc =>
                        new LARSLearningDeliveryCategory
                        {
                            CategoryRef = ldc.CategoryRef,
                            EffectiveFrom = ldc.EffectiveFrom,
                            EffectiveTo = ldc.EffectiveTo,
                        }).ToList(),
                        LARSFundings = ld.LarsFundings.Select(lf =>
                        new LARSFunding
                        {
                            FundingCategory = lf.FundingCategory,
                            EffectiveFrom = lf.EffectiveFrom,
                            EffectiveTo = lf.EffectiveTo,
                            RateUnWeighted = lf.RateUnWeighted,
                            RateWeighted = lf.RateWeighted,
                            WeightingFactor = lf.WeightingFactor,
                        }).ToList(),
                        LARSValidities = ld.LarsValidities.Select(lv =>
                        new LARSValidity
                        {
                            EffectiveFrom = lv.StartDate,
                            EffectiveTo = lv.EndDate,
                            LastNewStartDate = lv.LastNewStartDate,
                            ValidityCategory = lv.ValidityCategory,
                        }).ToList(),
                    }).ToListAsync(cancellationToken);

            foreach (var key in inputKeys)
            {
                var framework = await _larsContext.LARS_Frameworks
                    .Where(lf =>
                        lf.FworkCode == key.FworkCode
                    && lf.ProgType == key.ProgType
                    && lf.PwayCode == key.PwayCode)
                    .Select(lf => new LARSFramework
                    {
                        FworkCode = lf.FworkCode,
                        ProgType = lf.ProgType,
                        PwayCode = lf.PwayCode,
                        EffectiveFromNullable = lf.EffectiveFrom,
                        EffectiveTo = lf.EffectiveTo,
                        LARSFrameworkAim = lf.LarsFrameworkAims.Where(lfa => lfa.LearnAimRef == key.LearnAimRef)
                        .Select(lfa => new LARSFrameworkAim
                        {
                            EffectiveFrom = lfa.EffectiveFrom,
                            EffectiveTo = lfa.EffectiveTo,
                            FrameworkComponentType = lfa.FrameworkComponentType,
                        }).FirstOrDefault(),
                        LARSFrameworkApprenticeshipFundings = lf.LarsApprenticeshipFworkFundings.Select(laf =>
                        new LARSFrameworkApprenticeshipFunding
                        {
                            BandNumber = laf.BandNumber,
                            CareLeaverAdditionalPayment = laf.CareLeaverAdditionalPayment,
                            CoreGovContributionCap = laf.CoreGovContributionCap,
                            Duration = laf.Duration,
                            EffectiveFrom = laf.EffectiveFrom,
                            EffectiveTo = laf.EffectiveTo,
                            FundableWithoutEmployer = laf.FundableWithoutEmployer,
                            FundingCategory = laf.FundingCategory,
                            MaxEmployerLevyCap = laf.MaxEmployerLevyCap,
                            ReservedValue2 = laf.ReservedValue2,
                            ReservedValue3 = laf.ReservedValue3,
                            SixteenToEighteenEmployerAdditionalPayment = laf._1618employerAdditionalPayment,
                            SixteenToEighteenFrameworkUplift = laf._1618frameworkUplift,
                            SixteenToEighteenIncentive = laf._1618incentive,
                            SixteenToEighteenProviderAdditionalPayment = laf._1618providerAdditionalPayment,
                        }).ToList(),
                        LARSFrameworkCommonComponents = lf.LarsFrameworkCmnComps.Select(lcc =>
                        new LARSFrameworkCommonComponent
                        {
                            CommonComponent = lcc.CommonComponent,
                            EffectiveFrom = lcc.EffectiveFrom,
                            EffectiveTo = lcc.EffectiveTo,
                        }).ToList(),
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (framework != null)
                {
                    larsFrameworks.Add(new LARSFrameworkKey(key.LearnAimRef, framework));
                }
            }

            var frameworkDictionary = larsFrameworks.GroupBy(l => l.LearnAimRef).ToDictionary(k => k.Key, v => v.Select(l => l.LARSFramework).ToList());

            foreach (var learningDelivery in learningDeliveries)
            {
                frameworkDictionary.TryGetValue(learningDelivery.LearnAimRef, out var frameworks);

                learningDelivery.LARSFrameworks = frameworks;
            }

            return learningDeliveries;
        }
    }
}
