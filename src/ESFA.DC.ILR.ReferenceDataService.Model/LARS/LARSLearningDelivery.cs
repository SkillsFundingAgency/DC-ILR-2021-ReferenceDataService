using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSLearningDelivery : AbstractTimeBoundedEntity
    {
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

        public List<LARSAnnualValue> LARSAnnualValues { get; set; }

        public List<LARSCareerLearningPilot> LARSCareerLearningPilots { get; set; }

        public List<LARSFramework> LARSFrameworks { get; set; }

        public List<LARSFunding> LARSFundings { get; set; }

        public List<LARSLearningDeliveryCategory> LARSLearningDeliveryCategories { get; set; }

        public List<LARSValidity> LARSValidities { get; set; }
    }
}
