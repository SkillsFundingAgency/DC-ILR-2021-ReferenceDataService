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
                        LARSAnnualValues = ld.LarsAnnualValues.Select(la => LARSAnnualValueFromEntity(la)).ToList(),
                        LARSCareerLearningPilots = ld.LarsCareerLearningPilots.Select(lc => LARSCareerLearningPilotsFromEntity(lc)).ToList(),
                        LARSLearningDeliveryCategories = ld.LarsLearningDeliveryCategories.Select(ldc => LARSLearningDeliveryCategoriesFromEntity(ldc)).ToList(),
                        LARSFrameworkAims = ld.LarsFrameworkAims.Select(lfa => LARSFrameworkAimFromEntity(lfa)).ToList(),
                        LARSFundings = ld.LarsFundings.Select(lf => LARSFundingsFromEntity(lf)).ToList(),
                        LARSValidities = ld.LarsValidities.Select(lv => LARSValiditiesFromEntity(lv)).ToList()
                    }).ToListAsync(cancellationToken);
        }

        public LARSAnnualValue LARSAnnualValueFromEntity(LarsAnnualValue larsAnnualValue)
        {
            if (larsAnnualValue == null)
            {
                return new LARSAnnualValue();
            }

            return new LARSAnnualValue
            {
                BasicSkills = larsAnnualValue.BasicSkills,
                BasicSkillsType = larsAnnualValue.BasicSkillsType,
                EffectiveFrom = larsAnnualValue.EffectiveFrom,
                EffectiveTo = larsAnnualValue.EffectiveTo,
                FullLevel2EntitlementCategory = larsAnnualValue.FullLevel2EntitlementCategory,
                FullLevel3EntitlementCategory = larsAnnualValue.FullLevel3EntitlementCategory,
                FullLevel3Percent = larsAnnualValue.FullLevel3Percent
            };
        }

        public LARSCareerLearningPilot LARSCareerLearningPilotsFromEntity(LarsCareerLearningPilot larsCareerLearningPilot)
        {
            if (larsCareerLearningPilot == null)
            {
                return new LARSCareerLearningPilot();
            }

            return new LARSCareerLearningPilot
            {
                AreaCode = larsCareerLearningPilot.AreaCode,
                EffectiveFrom = larsCareerLearningPilot.EffectiveFrom,
                EffectiveTo = larsCareerLearningPilot.EffectiveTo,
                SubsidyRate = larsCareerLearningPilot.SubsidyRate
            };
        }

        public LARSLearningDeliveryCategory LARSLearningDeliveryCategoriesFromEntity(LarsLearningDeliveryCategory larsLearningDeliveryCategory)
        {
            if (larsLearningDeliveryCategory == null)
            {
                return new LARSLearningDeliveryCategory();
            }

            return new LARSLearningDeliveryCategory
            {
                CategoryRef = larsLearningDeliveryCategory.CategoryRef,
                EffectiveFrom = larsLearningDeliveryCategory.EffectiveFrom,
                EffectiveTo = larsLearningDeliveryCategory.EffectiveTo,
            };
        }

        public LARSFunding LARSFundingsFromEntity(LarsFunding larsFunding)
        {
            if (larsFunding == null)
            {
                return new LARSFunding();
            }

            return new LARSFunding
            {
                FundingCategory = larsFunding.FundingCategory,
                EffectiveFrom = larsFunding.EffectiveFrom,
                EffectiveTo = larsFunding.EffectiveTo,
                RateUnWeighted = larsFunding.RateUnWeighted,
                RateWeighted = larsFunding.RateWeighted,
                WeightingFactor = larsFunding.WeightingFactor,
            };
        }

        public LARSValidity LARSValiditiesFromEntity(LarsValidity larsValidity)
        {
            if (larsValidity == null)
            {
                return new LARSValidity();
            }

            return new LARSValidity
            {
                EffectiveFrom = larsValidity.StartDate,
                EffectiveTo = larsValidity.EndDate,
                LastNewStartDate = larsValidity.LastNewStartDate,
                ValidityCategory = larsValidity.ValidityCategory
            };
        }

        public LARSFrameworkAim LARSFrameworkAimFromEntity(LarsFrameworkAim larsFrameworkAim)
        {
            if (larsFrameworkAim == null)
            {
                return new LARSFrameworkAim();
            }

            return new LARSFrameworkAim
            {
                FrameworkComponentType = larsFrameworkAim.FrameworkComponentType,
                EffectiveFrom = larsFrameworkAim.EffectiveFrom,
                EffectiveTo = larsFrameworkAim.EffectiveTo,
                LARSFramework = LARSFrameworkFromEntity(larsFrameworkAim.LarsFramework)
            };
        }

        public LARSFramework LARSFrameworkFromEntity(LarsFramework larsFramework)
        {
            if (larsFramework == null)
            {
                return new LARSFramework();
            }

            return new LARSFramework
            {
                FworkCode = larsFramework.FworkCode,
                ProgType = larsFramework.ProgType,
                PwayCode = larsFramework.PwayCode,
                EffectiveFromNullable = larsFramework.EffectiveFrom,
                EffectiveTo = larsFramework.EffectiveTo,
                LARSFrameworkCommonComponents = larsFramework?.LarsFrameworkCmnComps?.Select(LARSFrameworkComCmpFromEntity).ToList(),
                LARSFrameworkApprenticeshipFundings = larsFramework?.LarsApprenticeshipFworkFundings?.Select(LARSFrameworkAppFundingFromEntity).ToList()
            };
        }

        public LARSFrameworkCommonComponent LARSFrameworkComCmpFromEntity(LarsFrameworkCmnComp larsFrameworkCmnComp)
        {
            if (larsFrameworkCmnComp == null)
            {
                return new LARSFrameworkCommonComponent();
            }

            return new LARSFrameworkCommonComponent
            {
                CommonComponent = larsFrameworkCmnComp.CommonComponent,
                EffectiveFrom = larsFrameworkCmnComp.EffectiveFrom,
                EffectiveTo = larsFrameworkCmnComp.EffectiveTo
            };
        }

        public LARSFrameworkApprenticeshipFunding LARSFrameworkAppFundingFromEntity(LarsApprenticeshipFworkFunding larsApprenticeshipFworkFunding)
        {
            if (larsApprenticeshipFworkFunding == null)
            {
                return new LARSFrameworkApprenticeshipFunding();
            }

            return new LARSFrameworkApprenticeshipFunding
            {
                BandNumber = larsApprenticeshipFworkFunding.BandNumber,
                CareLeaverAdditionalPayment = larsApprenticeshipFworkFunding.CareLeaverAdditionalPayment,
                CoreGovContributionCap = larsApprenticeshipFworkFunding.CoreGovContributionCap,
                Duration = larsApprenticeshipFworkFunding.Duration,
                EffectiveFrom = larsApprenticeshipFworkFunding.EffectiveFrom,
                EffectiveTo = larsApprenticeshipFworkFunding.EffectiveTo,
                FundableWithoutEmployer = larsApprenticeshipFworkFunding.FundableWithoutEmployer,
                FundingCategory = larsApprenticeshipFworkFunding.FundingCategory,
                MaxEmployerLevyCap = larsApprenticeshipFworkFunding.MaxEmployerLevyCap,
                ReservedValue2 = larsApprenticeshipFworkFunding.ReservedValue2,
                ReservedValue3 = larsApprenticeshipFworkFunding.ReservedValue3,
                SixteenToEighteenEmployerAdditionalPayment = larsApprenticeshipFworkFunding._1618employerAdditionalPayment,
                SixteenToEighteenFrameworkUplift = larsApprenticeshipFworkFunding._1618frameworkUplift,
                SixteenToEighteenIncentive = larsApprenticeshipFworkFunding._1618incentive,
                SixteenToEighteenProviderAdditionalPayment = larsApprenticeshipFworkFunding._1618providerAdditionalPayment,
            };
        }
    }
}
