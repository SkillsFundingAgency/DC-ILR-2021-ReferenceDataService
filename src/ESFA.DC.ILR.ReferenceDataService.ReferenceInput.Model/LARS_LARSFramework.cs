using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSFramework
    {
        public LARS_LARSFramework()
        {
            LARS_LARSFrameworkApprenticeshipFundings = new HashSet<LARS_LARSFrameworkApprenticeshipFunding>();
            LARS_LARSFrameworkCommonComponents = new HashSet<LARS_LARSFrameworkCommonComponent>();
        }

        public int Id { get; set; }
        public int FworkCode { get; set; }
        public int ProgType { get; set; }
        public int PwayCode { get; set; }
        public DateTime? EffectiveFromNullable { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LARSFrameworkAim_Id { get; set; }
        public int? LARS_LARSLearningDelivery_Id { get; set; }

        public virtual LARS_LARSFrameworkAim LARSFrameworkAim_ { get; set; }
        public virtual LARS_LARSLearningDelivery LARS_LARSLearningDelivery_ { get; set; }
        public virtual ICollection<LARS_LARSFrameworkApprenticeshipFunding> LARS_LARSFrameworkApprenticeshipFundings { get; set; }
        public virtual ICollection<LARS_LARSFrameworkCommonComponent> LARS_LARSFrameworkCommonComponents { get; set; }
    }
}
