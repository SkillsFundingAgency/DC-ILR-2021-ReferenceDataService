using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataMetaData
    {
        public int Id { get; set; }
        public DateTime DateGenerated { get; set; }
        public int? CollectionDates_Id { get; set; }
        public int? ReferenceDataVersions_Id { get; set; }

        public virtual MetaDataIlrCollectionDate CollectionDates_ { get; set; }
        public virtual MetaDataReferenceDataVersion ReferenceDataVersions_ { get; set; }
    }
}
