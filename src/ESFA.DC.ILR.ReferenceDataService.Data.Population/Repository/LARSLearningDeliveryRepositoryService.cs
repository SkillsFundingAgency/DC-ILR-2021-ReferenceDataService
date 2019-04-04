using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class LarsLearningDeliveryRepositoryService : ILarsLearningDeliveryRepositoryService
    {
        private readonly ILARSContext _larsContext;

        public LarsLearningDeliveryRepositoryService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyCollection<LARSLearningDelivery>> RetrieveAsync(IReadOnlyCollection<string> learnAimRefs, CancellationToken cancellationToken)
        {
            return await _larsContext.LARS_LearningDeliveries
                .Include(ld => ld.LarsAnnualValues)
                .Include(ld => ld.LarsCareerLearningPilots)
                .Include(ld => ld.LarsLearningDeliveryCategories)
                .Include(ld => ld.LarsFrameworkAims)
                    .ThenInclude(lfa => lfa.LarsFramework)
                .Include(ld => ld.LarsFrameworkAims)
                    .ThenInclude(lfa => lfa.LarsFramework)
                    .ThenInclude(lf => lf.LarsFrameworkCmnComps)
                .Include(ld => ld.LarsFrameworkAims)
                    .ThenInclude(lfa => lfa.LarsFramework)
                    .ThenInclude(lf => lf.LarsApprenticeshipFworkFundings)
                .Include(ld => ld.LarsFundings)
                .Include(ld => ld.LarsValidities)
                .Where(l => learnAimRefs.Contains(l.LearnAimRef))
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
                            FullLevel3Percent = la.FullLevel3Percent
                        }).ToList(),
                        LARSCareerLearningPilots = ld.LarsCareerLearningPilots.Select(lc =>
                        new LARSCareerLearningPilot
                        {
                            AreaCode = lc.AreaCode,
                            EffectiveFrom = lc.EffectiveFrom,
                            EffectiveTo = lc.EffectiveTo,
                            SubsidyRate = lc.SubsidyRate
                        }).ToList(),
                        LARSLearningDeliveryCategories = ld.LarsLearningDeliveryCategories.Select(ldc =>
                        new LARSLearningDeliveryCategory
                        {
                            CategoryRef = ldc.CategoryRef,
                            EffectiveFrom = ldc.EffectiveFrom,
                            EffectiveTo = ldc.EffectiveTo,
                        }).ToList(),
                        LARSFrameworkAims = ld.LarsFrameworkAims.Select(lfa =>
                        new LARSFrameworkAim
                        {
                            FworkCode = lfa.FworkCode,
                            ProgType = lfa.ProgType,
                            PwayCode = lfa.PwayCode,
                            FrameworkComponentType = lfa.FrameworkComponentType,
                            EffectiveFrom = lfa.EffectiveFrom,
                            EffectiveTo = lfa.EffectiveTo,
                            LARSFramework = new LARSFramework
                            {
                                EffectiveFromNullable = lfa.LarsFramework.EffectiveFrom,
                                EffectiveTo = lfa.LarsFramework.EffectiveTo,
                                LARSFrameworkApprenticeshipFundings = lfa.LarsFramework.LarsApprenticeshipFworkFundings.Select(laf =>
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
                                LARSFrameworkCommonComponents = lfa.LarsFramework.LarsFrameworkCmnComps.Select(lcc =>
                                new LARSFrameworkCommonComponent
                                {
                                    CommonComponent = lcc.CommonComponent,
                                    EffectiveFrom = lcc.EffectiveFrom,
                                    EffectiveTo = lcc.EffectiveTo
                                }).ToList()
                            }
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
                            ValidityCategory = lv.ValidityCategory
                        }).ToList()
                    }).ToListAsync(cancellationToken);
        }
    }
}
