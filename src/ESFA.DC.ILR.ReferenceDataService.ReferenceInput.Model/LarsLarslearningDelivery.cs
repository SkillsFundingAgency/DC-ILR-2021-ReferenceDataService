using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarslearningDelivery
    {
        public LarsLarslearningDelivery()
        {
            LarsLarsannualValues = new HashSet<LarsLarsannualValue>();
            LarsLarsframeworks = new HashSet<LarsLarsframework>();
            LarsLarsfundings = new HashSet<LarsLarsfunding>();
            LarsLarslearningDeliveryCategories = new HashSet<LarsLarslearningDeliveryCategory>();
            LarsLarsvalidities = new HashSet<LarsLarsvalidity>();
        }

        public int Id { get; set; }
        public string LearnAimRef { get; set; }
        public string LearnAimRefType { get; set; }
        public string LearnAimRefTitle { get; set; }
        public int? EnglPrscId { get; set; }
        public string NotionalNvqlevel { get; set; }
        public string NotionalNvqlevelv2 { get; set; }
        public int? FrameworkCommonComponent { get; set; }
        public string LearnDirectClassSystemCode1 { get; set; }
        public string LearnDirectClassSystemCode2 { get; set; }
        public string LearnDirectClassSystemCode3 { get; set; }
        public decimal? SectorSubjectAreaTier1 { get; set; }
        public decimal? SectorSubjectAreaTier2 { get; set; }
        public string LearningDeliveryGenre { get; set; }
        public int? RegulatedCreditValue { get; set; }
        public string EnglandFehestatus { get; set; }
        public string AwardOrgCode { get; set; }
        public int? Efacoftype { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<LarsLarsannualValue> LarsLarsannualValues { get; set; }
        public virtual ICollection<LarsLarsframework> LarsLarsframeworks { get; set; }
        public virtual ICollection<LarsLarsfunding> LarsLarsfundings { get; set; }
        public virtual ICollection<LarsLarslearningDeliveryCategory> LarsLarslearningDeliveryCategories { get; set; }
        public virtual ICollection<LarsLarsvalidity> LarsLarsvalidities { get; set; }
    }
}
