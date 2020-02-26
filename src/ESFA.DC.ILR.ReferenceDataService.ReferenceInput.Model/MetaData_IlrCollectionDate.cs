using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_IlrCollectionDate
    {
        public MetaData_IlrCollectionDate()
        {
            MetaData_MetaDatas = new HashSet<MetaData_MetaData>();
        }

        public int Id { get; set; }

        public virtual ICollection<MetaData_MetaData> MetaData_MetaDatas { get; set; }
    }
}
