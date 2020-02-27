using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSFrameworkCommonComponent
    {
        public int Id { get; set; }
        public int CommonComponent { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LARS_LARSFrameworkDesktop_Id { get; set; }
        public int? LARS_LARSFramework_Id { get; set; }

        public virtual LARS_LARSFrameworkDesktop LARS_LARSFrameworkDesktop_ { get; set; }
        public virtual LARS_LARSFramework LARS_LARSFramework_ { get; set; }
    }
}
