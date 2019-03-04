using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class LarsLearningDeliveryService : IRetrievalService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>>
    {
        private readonly ILARSContext _larsContext;

        public LarsLearningDeliveryService()
        {
        }

        public LarsLearningDeliveryService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyDictionary<string, LARSLearningDelivery>> RetrieveAsync(IReadOnlyCollection<string> learnAimRefs, CancellationToken cancellationToken)
        {
            var larsLearningDeliveries = await _larsContext.LARS_LearningDeliveries
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
                        LARSAnnualValues = ld.LarsAnnualValues.Select(LARSAnnualValueFromEntity).ToList(),
                        LARSCareerLearningPilots = ld.LarsCareerLearningPilots.Select(LARSCareerLearningPilotsFromEntity).ToList(),
                        LARSLearningDeliveryCategories = ld.LarsLearningDeliveryCategories.Select(LARSLearningDeliveryCategoriesFromEntity).ToList(),
                        LARSFrameworkAims = ld.LarsFrameworkAims.Select(LARSFrameworkAimFromEntity).ToList(),
                        LARSFundings = ld.LarsFundings.Select(LARSFundingsFromEntity).ToList(),
                        LARSValidities = ld.LarsValidities.Select(LARSValiditiesFromEntity).ToList()
                    }).ToListAsync(cancellationToken);

            return larsLearningDeliveries
                .GroupBy(ld => ld.LearnAimRef)
                .ToDictionary(key => key.Key, value => value.FirstOrDefault());
        }

        private LARSAnnualValue LARSAnnualValueFromEntity(LarsAnnualValue larsAnnualValue)
        {
            var annualValue = new LARSAnnualValue
            {
                BasicSkills = larsAnnualValue.BasicSkills,
                BasicSkillsType = larsAnnualValue.BasicSkillsType,
                FullLevel2EntitlementCategory = larsAnnualValue.FullLevel2EntitlementCategory,
                FullLevel3EntitlementCategory = larsAnnualValue.FullLevel3EntitlementCategory,
                FullLevel3Percent = larsAnnualValue.FullLevel3Percent
            };

            return annualValue ?? new LARSAnnualValue();
        }

        private LARSCareerLearningPilot LARSCareerLearningPilotsFromEntity(LarsCareerLearningPilot larsCareerLearningPilot)
        {
            var careerLearningPilot = new LARSCareerLearningPilot
            {
                AreaCode = larsCareerLearningPilot.AreaCode,
                EffectiveFrom = larsCareerLearningPilot.EffectiveFrom,
                EffectiveTo = larsCareerLearningPilot.EffectiveTo,
                SubsidyRate = larsCareerLearningPilot.SubsidyRate
            };

            return careerLearningPilot ?? new LARSCareerLearningPilot();
        }

        private LARSLearningDeliveryCategory LARSLearningDeliveryCategoriesFromEntity(LarsLearningDeliveryCategory larsLearningDeliveryCategory)
        {
            var category = new LARSLearningDeliveryCategory
            {
                CategoryRef = larsLearningDeliveryCategory.CategoryRef,
                EffectiveFrom = larsLearningDeliveryCategory.EffectiveFrom,
                EffectiveTo = larsLearningDeliveryCategory.EffectiveTo,
            };

            return category ?? new LARSLearningDeliveryCategory();
        }

        private LARSFunding LARSFundingsFromEntity(LarsFunding larsFunding)
        {
            var funding = new LARSFunding
            {
                FundingCategory = larsFunding.FundingCategory,
                EffectiveFrom = larsFunding.EffectiveFrom,
                EffectiveTo = larsFunding.EffectiveTo,
                RateUnWeighted = larsFunding.RateUnWeighted,
                RateWeighted = larsFunding.RateWeighted,
                WeightingFactor = larsFunding.WeightingFactor,
            };

            return funding ?? new LARSFunding();
        }

        private LARSValidity LARSValiditiesFromEntity(LarsValidity larsValidity)
        {
            var validity = new LARSValidity
            {
                EffectiveFrom = larsValidity.StartDate,
                EffectiveTo = larsValidity.EndDate,
                LastNewStartDate = larsValidity.LastNewStartDate,
                ValidityCategory = larsValidity.ValidityCategory
            };

            return validity ?? new LARSValidity();
        }

        private LARSFrameworkAim LARSFrameworkAimFromEntity(LarsFrameworkAim larsFrameworkAim)
        {
            var frameworkAim = new LARSFrameworkAim
            {
                FrameworkComponentType = larsFrameworkAim.FrameworkComponentType,
                EffectiveFrom = larsFrameworkAim.EffectiveFrom,
                EffectiveTo = larsFrameworkAim.EffectiveTo,
                LARSFramework = larsFrameworkAim?.LarsFramework == null ? new LARSFramework() : LARSFrameworkFromEntity(larsFrameworkAim?.LarsFramework)
            };

            return frameworkAim ?? new LARSFrameworkAim();
        }

        private LARSFramework LARSFrameworkFromEntity(LarsFramework larsFramework)
        {
            var framework = new LARSFramework
            {
                FworkCode = larsFramework.FworkCode,
                ProgType = larsFramework.ProgType,
                PwayCode = larsFramework.PwayCode,
                EffectiveFromNullable = larsFramework.EffectiveFrom,
                EffectiveTo = larsFramework.EffectiveTo,
                LARSFrameworkCommonComponents = larsFramework?.LarsFrameworkCmnComps?.Select(LARSFrameworkComCmpFromEntity).ToList(),
                LARSFrameworkApprenticeshipFundings = larsFramework?.LarsApprenticeshipFworkFundings?.Select(LARSFrameworkAppFundingFromEntity).ToList()
            };

            return framework ?? new LARSFramework();
        }

        private LARSFrameworkCommonComponent LARSFrameworkComCmpFromEntity(LarsFrameworkCmnComp larsFrameworkCmnComp)
        {
            var frameworkCmnComp = new LARSFrameworkCommonComponent
            {
                CommonComponent = larsFrameworkCmnComp.CommonComponent,
                EffectiveFrom = larsFrameworkCmnComp.EffectiveFrom,
                EffectiveTo = larsFrameworkCmnComp.EffectiveTo
            };

            return frameworkCmnComp ?? new LARSFrameworkCommonComponent();
        }

        private LARSFrameworkApprenticeshipFunding LARSFrameworkAppFundingFromEntity(LarsApprenticeshipFworkFunding larsApprenticeshipFworkFunding)
        {
            var larsAppFworkFunding = new LARSFrameworkApprenticeshipFunding
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
                SixteenToEighteenProviderAdditionalPayment = larsApprenticeshipFworkFunding._1618providerAdditionalPayment
            };

            return larsAppFworkFunding ?? new LARSFrameworkApprenticeshipFunding();
        }
    }
}
