using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataIlrCollectionDates
    {
        public MetaDataIlrCollectionDates()
        {
            MetaDataMetaData = new HashSet<MetaDataMetaData>();
        }

        public int Id { get; set; }

        public virtual ICollection<MetaDataMetaData> MetaDataMetaData { get; set; }
    }
}
