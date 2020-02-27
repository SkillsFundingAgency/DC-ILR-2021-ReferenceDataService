using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataIlrCollectionDate
    {
        public MetaDataIlrCollectionDate()
        {
            MetaDataMetaDatas = new HashSet<MetaDataMetaData>();
        }

        public int Id { get; set; }

        public virtual ICollection<MetaDataMetaData> MetaDataMetaDatas { get; set; }
    }
}
