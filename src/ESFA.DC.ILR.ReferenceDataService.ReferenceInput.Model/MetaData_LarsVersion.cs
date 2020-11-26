using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_LarsVersion
    {
        public MetaData_LarsVersion()
        {
            MetaData_ReferenceDataVersions = new HashSet<MetaData_ReferenceDataVersion>();
        }

        public int Id { get; set; }
        public string Version { get; set; }

        public virtual ICollection<MetaData_ReferenceDataVersion> MetaData_ReferenceDataVersions { get; set; }
    }
}
