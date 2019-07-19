using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
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
            var learningDeliveries = await BuildLARSLearningDelvieries(cancellationToken);
            var larsAnnualValuesDictionary = await BuildLARSAnnualValueDictionary(cancellationToken);
            var larsCareerLearningPilotsDictionary = await BuildLARSCareerLearningPilotsDictionary(cancellationToken);
            var larsLearningDeliveryCategoriesDictionary = await BuildLARSLearningDeliveryCategoryDictionary(cancellationToken);
            var larsFundingsDictionary = await BuildLARSFundingDictionary(cancellationToken);
            var larsValiditiesDictionary = await BuildLARSValidityDictionary(cancellationToken);

            var defaultLarsAnnualValues = new List<LARSAnnualValue>();
            var defaultLarsCareerLearningPilots = new List<LARSCareerLearningPilot>();
            var defaultLarsLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>();
            var defaultLarsFundings = new List<LARSFunding>();
            var defaultLarsValidities = new List<LARSValidity>();
            var defaultLarsFrameworks = new List<LARSFramework>();

            foreach (var learningDelivery in learningDeliveries)
            {
                learningDelivery.LARSAnnualValues = larsAnnualValuesDictionary.TryGetValue(learningDelivery.LearnAimRef, out var annualValues) ? annualValues : defaultLarsAnnualValues;
                learningDelivery.LARSCareerLearningPilots = larsCareerLearningPilotsDictionary.TryGetValue(learningDelivery.LearnAimRef, out var careerLearningPilots) ? careerLearningPilots : defaultLarsCareerLearningPilots;
                learningDelivery.LARSLearningDeliveryCategories = larsLearningDeliveryCategoriesDictionary.TryGetValue(learningDelivery.LearnAimRef, out var categories) ? categories : defaultLarsLearningDeliveryCategories;
                learningDelivery.LARSFundings = larsFundingsDictionary.TryGetValue(learningDelivery.LearnAimRef, out var fundings) ? fundings : defaultLarsFundings;
                learningDelivery.LARSValidities = larsValiditiesDictionary.TryGetValue(learningDelivery.LearnAimRef, out var validities) ? validities : defaultLarsValidities;
            }

            return learningDeliveries;
        }

        private async Task<List<LARSLearningDelivery>> BuildLARSLearningDelvieries(CancellationToken cancellationToken)
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
                  SectorSubjectAreaTier2 = ld.SectorSubjectAreaTier2
              }).ToListAsync(cancellationToken);
        }

        private async Task<Dictionary<string, List<LARSAnnualValue>>> BuildLARSAnnualValueDictionary(CancellationToken cancellationToken)
        {
            var larsAnnualValuesList = await _larsContext.LARS_AnnualValues
            .Select(la => new LARSAnnualValue
            {
                LearnAimRef = la.LearnAimRef,
                BasicSkills = la.BasicSkills,
                BasicSkillsType = la.BasicSkillsType,
                EffectiveFrom = la.EffectiveFrom,
                EffectiveTo = la.EffectiveTo,
                FullLevel2EntitlementCategory = la.FullLevel2EntitlementCategory,
                FullLevel3EntitlementCategory = la.FullLevel3EntitlementCategory,
                FullLevel2Percent = la.FullLevel2Percent,
                FullLevel3Percent = la.FullLevel3Percent,
            }).ToListAsync(cancellationToken);

            return larsAnnualValuesList
                .GroupBy(l => l.LearnAimRef, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                k => k.Key,
                v => v.Select(l => l).ToList(),
                StringComparer.OrdinalIgnoreCase);
        }

        private async Task<Dictionary<string, List<LARSCareerLearningPilot>>> BuildLARSCareerLearningPilotsDictionary(CancellationToken cancellationToken)
        {
            var larsLearningDeliveryCategoriesList = await _larsContext.LARS_CareerLearningPilots
            .Select(lc => new LARSCareerLearningPilot
            {
                LearnAimRef = lc.LearnAimRef,
                AreaCode = lc.AreaCode,
                EffectiveFrom = lc.EffectiveFrom,
                EffectiveTo = lc.EffectiveTo,
                SubsidyRate = lc.SubsidyRate,
            }).ToListAsync(cancellationToken);

            return larsLearningDeliveryCategoriesList
                .GroupBy(l => l.LearnAimRef, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                k => k.Key,
                v => v.Select(l => l).ToList(),
                StringComparer.OrdinalIgnoreCase);
        }

        private async Task<Dictionary<string, List<LARSLearningDeliveryCategory>>> BuildLARSLearningDeliveryCategoryDictionary(CancellationToken cancellationToken)
        {
            var larsLearningDeliveryCategoriesList = await _larsContext.LARS_LearningDeliveryCategories
            .Select(ldc => new LARSLearningDeliveryCategory
            {
                LearnAimRef = ldc.LearnAimRef,
                CategoryRef = ldc.CategoryRef,
                EffectiveFrom = ldc.EffectiveFrom,
                EffectiveTo = ldc.EffectiveTo,
            }).ToListAsync(cancellationToken);

            return larsLearningDeliveryCategoriesList
                .GroupBy(l => l.LearnAimRef, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                k => k.Key,
                v => v.Select(l => l).ToList(),
                StringComparer.OrdinalIgnoreCase);
        }

        private async Task<Dictionary<string, List<LARSFunding>>> BuildLARSFundingDictionary(CancellationToken cancellationToken)
        {
            var larsFundingList = await _larsContext.LARS_Fundings
            .Select(lf => new LARSFunding
            {
                LearnAimRef = lf.LearnAimRef,
                FundingCategory = lf.FundingCategory,
                EffectiveFrom = lf.EffectiveFrom,
                EffectiveTo = lf.EffectiveTo,
                RateUnWeighted = lf.RateUnWeighted,
                RateWeighted = lf.RateWeighted,
                WeightingFactor = lf.WeightingFactor,
            }).ToListAsync(cancellationToken);

            return larsFundingList
                .GroupBy(l => l.LearnAimRef, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                k => k.Key,
                v => v.Select(l => l).ToList(),
                StringComparer.OrdinalIgnoreCase);
        }

        private async Task<Dictionary<string, List<LARSValidity>>> BuildLARSValidityDictionary(CancellationToken cancellationToken)
        {
            var larsValiditiesList = await _larsContext.LARS_Validities
            .Select(lv => new LARSValidity
            {
                LearnAimRef = lv.LearnAimRef,
                EffectiveFrom = lv.StartDate,
                EffectiveTo = lv.EndDate,
                LastNewStartDate = lv.LastNewStartDate,
                ValidityCategory = lv.ValidityCategory,
            }).ToListAsync(cancellationToken);

            return larsValiditiesList
                .GroupBy(l => l.LearnAimRef, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                k => k.Key,
                v => v.Select(l => l).ToList(),
                StringComparer.OrdinalIgnoreCase);
        }
    }
}
