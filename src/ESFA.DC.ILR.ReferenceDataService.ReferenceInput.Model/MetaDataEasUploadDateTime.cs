using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataEasUploadDateTime
    {
        public MetaDataEasUploadDateTime()
        {
            MetaDataReferenceDataVersions = new HashSet<MetaDataReferenceDataVersion>();
        }

        public int Id { get; set; }
        public DateTime? UploadDateTime { get; set; }

        public virtual ICollection<MetaDataReferenceDataVersion> MetaDataReferenceDataVersions { get; set; }
    }
}
