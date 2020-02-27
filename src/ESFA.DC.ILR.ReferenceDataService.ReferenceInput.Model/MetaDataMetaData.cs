using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataMetaData
    {
        public int Id { get; set; }
        public DateTime DateGenerated { get; set; }
        public int? CollectionDatesId { get; set; }
        public int? ReferenceDataVersionsId { get; set; }

        public virtual MetaDataIlrCollectionDate CollectionDates { get; set; }
        public virtual MetaDataReferenceDataVersion ReferenceDataVersions { get; set; }
    }
}
