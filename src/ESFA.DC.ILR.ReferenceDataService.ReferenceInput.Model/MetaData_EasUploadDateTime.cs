using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_EasUploadDateTime
    {
        public MetaData_EasUploadDateTime()
        {
            MetaData_ReferenceDataVersions = new HashSet<MetaData_ReferenceDataVersion>();
        }

        public int Id { get; set; }
        public DateTime? UploadDateTime { get; set; }

        public virtual ICollection<MetaData_ReferenceDataVersion> MetaData_ReferenceDataVersions { get; set; }
    }
}
