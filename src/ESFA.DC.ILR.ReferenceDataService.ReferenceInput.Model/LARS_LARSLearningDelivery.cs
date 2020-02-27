using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSLearningDelivery
    {
        public LARS_LARSLearningDelivery()
        {
            LARS_LARSAnnualValues = new HashSet<LARS_LARSAnnualValue>();
            LARS_LARSFrameworks = new HashSet<LARS_LARSFramework>();
            LARS_LARSFundings = new HashSet<LARS_LARSFunding>();
            LARS_LARSLearningDeliveryCategories = new HashSet<LARS_LARSLearningDeliveryCategory>();
            LARS_LARSValidities = new HashSet<LARS_LARSValidity>();
        }

        public int Id { get; set; }
        public string LearnAimRef { get; set; }
        public string LearnAimRefType { get; set; }
        public string LearnAimRefTitle { get; set; }
        public int? EnglPrscID { get; set; }
        public string NotionalNVQLevel { get; set; }
        public string NotionalNVQLevelv2 { get; set; }
        public int? FrameworkCommonComponent { get; set; }
        public string LearnDirectClassSystemCode1 { get; set; }
        public string LearnDirectClassSystemCode2 { get; set; }
        public string LearnDirectClassSystemCode3 { get; set; }
        public decimal? SectorSubjectAreaTier1 { get; set; }
        public decimal? SectorSubjectAreaTier2 { get; set; }
        public string LearningDeliveryGenre { get; set; }
        public int? RegulatedCreditValue { get; set; }
        public string EnglandFEHEStatus { get; set; }
        public string AwardOrgCode { get; set; }
        public int? EFACOFType { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<LARS_LARSAnnualValue> LARS_LARSAnnualValues { get; set; }
        public virtual ICollection<LARS_LARSFramework> LARS_LARSFrameworks { get; set; }
        public virtual ICollection<LARS_LARSFunding> LARS_LARSFundings { get; set; }
        public virtual ICollection<LARS_LARSLearningDeliveryCategory> LARS_LARSLearningDeliveryCategories { get; set; }
        public virtual ICollection<LARS_LARSValidity> LARS_LARSValidities { get; set; }
    }
}
