using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarslearningDelivery
    {
        public LarsLarslearningDelivery()
        {
            LarsLarsannualValue = new HashSet<LarsLarsannualValue>();
            LarsLarsframework = new HashSet<LarsLarsframework>();
            LarsLarsfunding = new HashSet<LarsLarsfunding>();
            LarsLarslearningDeliveryCategory = new HashSet<LarsLarslearningDeliveryCategory>();
            LarsLarsvalidity = new HashSet<LarsLarsvalidity>();
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

        public virtual ICollection<LarsLarsannualValue> LarsLarsannualValue { get; set; }
        public virtual ICollection<LarsLarsframework> LarsLarsframework { get; set; }
        public virtual ICollection<LarsLarsfunding> LarsLarsfunding { get; set; }
        public virtual ICollection<LarsLarslearningDeliveryCategory> LarsLarslearningDeliveryCategory { get; set; }
        public virtual ICollection<LarsLarsvalidity> LarsLarsvalidity { get; set; }
    }
}
