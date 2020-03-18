using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_MetaData
    {
        public int Id { get; set; }
        public DateTime DateGenerated { get; set; }
        public int? ReferenceDataVersions_Id { get; set; }

        public virtual MetaData_ReferenceDataVersion ReferenceDataVersions_ { get; set; }
    }
}
