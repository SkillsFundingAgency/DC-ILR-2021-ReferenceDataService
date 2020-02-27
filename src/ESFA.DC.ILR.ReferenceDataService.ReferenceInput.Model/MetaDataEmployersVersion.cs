using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataEmployersVersion
    {
        public MetaDataEmployersVersion()
        {
            MetaDataReferenceDataVersions = new HashSet<MetaDataReferenceDataVersion>();
        }

        public int Id { get; set; }
        public string Version { get; set; }

        public virtual ICollection<MetaDataReferenceDataVersion> MetaDataReferenceDataVersions { get; set; }
    }
}
