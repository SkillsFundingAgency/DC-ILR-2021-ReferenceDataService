using System;
using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public class MetaData
    {
        public DateTime DateGenerated { get; set; }

        public ReferenceDataVersion ReferenceDataVersions { get; set; }

        public IReadOnlyCollection<ValidationError> ValidationErrors { get; set; }

        public IReadOnlyCollection<ValidationRule> ValidationRules { get; set; }

        public IReadOnlyCollection<Lookup> Lookups { get; set; }

        public IlrCollectionDates CollectionDates { get; set; }
    }
}
