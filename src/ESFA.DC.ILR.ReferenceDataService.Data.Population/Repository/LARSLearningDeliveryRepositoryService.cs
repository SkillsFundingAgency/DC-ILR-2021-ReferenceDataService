using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class LarsLearningDeliveryRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>
    {
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;

        public LarsLearningDeliveryRepositoryService(IDbContextFactory<ILARSContext> larsContextFactory)
        {
            _larsContextFactory = larsContextFactory;
        }

        public async Task<IReadOnlyCollection<LARSLearningDelivery>> RetrieveAsync(IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, CancellationToken cancellationToken)
        {
            using (var context = _larsContextFactory.Create())
            {
                var larsFrameworks = new List<LARSFrameworkKey>();

                var learningDeliveries = await BuildLARSLearningDelvieries(inputKeys, context, cancellationToken);
                var larsAnnualValuesDictionary = await BuildLARSAnnualValueDictionary(inputKeys, context, cancellationToken);
                var larsLearningDeliveryCategoriesDictionary = await BuildLARSLearningDeliveryCategoryDictionary(inputKeys, context, cancellationToken);
                var larsFundingsDictionary = await BuildLARSFundingDictionary(inputKeys, context, cancellationToken);
                var larsValiditiesDictionary = await BuildLARSValidityDictionary(inputKeys, context, cancellationToken);
                var frameworkAimDictionary = await BuildLARSFrameworkAimDictionary(inputKeys, context, cancellationToken);

                foreach (var key in inputKeys)
                {
                    var framework = await context.LARS_Frameworks
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
                        framework.LARSFrameworkAim = frameworkAimDictionary.TryGetValue(key, out var frameworkAim) ? frameworkAim : null;

                        larsFrameworks.Add(new LARSFrameworkKey(key.LearnAimRef, framework));
                    }
                }

                var frameworkDictionary = larsFrameworks.GroupBy(l => l.LearnAimRef).ToDictionary(k => k.Key, v => v.Select(l => l.LARSFramework).ToList(), StringComparer.OrdinalIgnoreCase);

                var defaultLarsAnnualValues = new List<LARSAnnualValue>();
                var defaultLarsLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>();
                var defaultLarsFundings = new List<LARSFunding>();
                var defaultLarsValidities = new List<LARSValidity>();
                var defaultLarsFrameworks = new List<LARSFramework>();

                foreach (var learningDelivery in learningDeliveries)
                {
                    learningDelivery.LARSAnnualValues = larsAnnualValuesDictionary.TryGetValue(learningDelivery.LearnAimRef, out var annualValues) ? annualValues : defaultLarsAnnualValues;
                    learningDelivery.LARSLearningDeliveryCategories = larsLearningDeliveryCategoriesDictionary.TryGetValue(learningDelivery.LearnAimRef, out var categories) ? categories : defaultLarsLearningDeliveryCategories;
                    learningDelivery.LARSFundings = larsFundingsDictionary.TryGetValue(learningDelivery.LearnAimRef, out var fundings) ? fundings : defaultLarsFundings;
                    learningDelivery.LARSValidities = larsValiditiesDictionary.TryGetValue(learningDelivery.LearnAimRef, out var validities) ? validities : defaultLarsValidities;
                    learningDelivery.LARSFrameworks = frameworkDictionary.TryGetValue(learningDelivery.LearnAimRef, out var frameworks) ? frameworks : defaultLarsFrameworks;
                }

                return learningDeliveries;
            }
        }

        private async Task<List<LARSLearningDelivery>> BuildLARSLearningDelvieries(
            IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, ILARSContext context, CancellationToken cancellationToken)
        {
            return await context.LARS_LearningDeliveries
                .Where(l => inputKeys.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
                .Select(
                ld => new LARSLearningDelivery
                {
                    LearnAimRef = ld.LearnAimRef,
                    LearnAimRefTitle = ld.LearnAimRefTitle,
                    LearnAimRefType = ld.LearnAimRefType,
                    LearnAimRefTypeDesc = ld.LearnAimRefTypeNavigation.LearnAimRefTypeDesc,
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
                    GuidedLearningHours = ld.GuidedLearningHours,
                    NotionalNVQLevel = ld.NotionalNvqlevel,
                    NotionalNVQLevelv2 = ld.NotionalNvqlevelv2,
                    RegulatedCreditValue = ld.RegulatedCreditValue,
                    SectorSubjectAreaTier1 = ld.SectorSubjectAreaTier1,
                    SectorSubjectAreaTier2 = ld.SectorSubjectAreaTier2
                }).ToListAsync(cancellationToken);
        }

        private async Task<Dictionary<string, List<LARSAnnualValue>>> BuildLARSAnnualValueDictionary(
            IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, ILARSContext context, CancellationToken cancellationToken)
        {
            var larsAnnualValuesList = await context.LARS_AnnualValues
                 .Where(l => inputKeys.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
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

        private async Task<Dictionary<string, List<LARSLearningDeliveryCategory>>> BuildLARSLearningDeliveryCategoryDictionary(
            IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, ILARSContext context, CancellationToken cancellationToken)
        {
            var larsLearningDeliveryCategoriesList = await context.LARS_LearningDeliveryCategories
                 .Where(l => inputKeys.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
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

        private async Task<Dictionary<string, List<LARSFunding>>> BuildLARSFundingDictionary(
            IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, ILARSContext context, CancellationToken cancellationToken)
        {
            var larsFundingList = await context.LARS_Fundings
                 .Where(l => inputKeys.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
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

        private async Task<Dictionary<string, List<LARSValidity>>> BuildLARSValidityDictionary(
            IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, ILARSContext context, CancellationToken cancellationToken)
        {
            var larsValiditiesList = await context.LARS_Validities
                 .Where(l => inputKeys.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
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

        private async Task<Dictionary<LARSLearningDeliveryKey, LARSFrameworkAim>> BuildLARSFrameworkAimDictionary(
           IReadOnlyCollection<LARSLearningDeliveryKey> inputKeys, ILARSContext context, CancellationToken cancellationToken)
        {
            return await context.LARS_FrameworkAims
                .Where(l => inputKeys.Select(lldk => lldk.LearnAimRef).Contains(l.LearnAimRef, StringComparer.OrdinalIgnoreCase))
                .GroupBy(ld => new LARSLearningDeliveryKey(ld.LearnAimRef, ld.FworkCode, ld.ProgType, ld.PwayCode))
               .ToDictionaryAsync(
                k => k.Key,
                v => v.Select(fa => new LARSFrameworkAim
                {
                    LearnAimRef = fa.LearnAimRef.ToUpper(),
                    FrameworkComponentType = fa.FrameworkComponentType,
                    EffectiveFrom = fa.EffectiveFrom,
                    EffectiveTo = fa.EffectiveTo,
                }).FirstOrDefault());
        }
    }
}
