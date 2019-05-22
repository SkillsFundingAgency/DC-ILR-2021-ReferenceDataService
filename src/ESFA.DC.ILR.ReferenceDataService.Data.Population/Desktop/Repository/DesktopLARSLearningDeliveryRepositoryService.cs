using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop.Repository
{
    public class DesktopLarsLearningDeliveryRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>>
    {
        private readonly ILARSContext _larsContext;

        public DesktopLarsLearningDeliveryRepositoryService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyCollection<LARSLearningDelivery>> RetrieveAsync(CancellationToken cancellationToken)
        {
            return await _larsContext.LARS_LearningDeliveries
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
        }
    }
}
