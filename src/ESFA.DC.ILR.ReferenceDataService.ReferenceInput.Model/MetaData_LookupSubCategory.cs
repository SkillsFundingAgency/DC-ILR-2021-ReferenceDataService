using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_LookupSubCategory
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? MetaData_Lookup_Id { get; set; }

        public virtual MetaData_Lookup MetaData_Lookup_ { get; set; }
    }
}
