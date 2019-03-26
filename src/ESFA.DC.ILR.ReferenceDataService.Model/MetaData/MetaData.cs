using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public class MetaData
    {
        public ReferenceDataVersion ReferenceDataVersions { get; set; }

        public IReadOnlyCollection<ValidationError> ValidationErrors { get; set; }
    }
}
