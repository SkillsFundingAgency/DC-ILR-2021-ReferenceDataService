using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSStandardCommonComponent
    {
        public int Id { get; set; }
        public int CommonComponent { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LARS_LARSStandard_Id { get; set; }

        public virtual LARS_LARSStandard LARS_LARSStandard_ { get; set; }
    }
}
