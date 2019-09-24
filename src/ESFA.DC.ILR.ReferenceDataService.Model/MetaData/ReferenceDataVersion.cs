using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public class ReferenceDataVersion
    {
        public CoFVersion CoFVersion { get; set; }

        public EmployersVersion Employers { get; set; }

        public LarsVersion LarsVersion { get; set; }

        public OrganisationsVersion OrganisationsVersion { get; set; }

        public PostcodesVersion PostcodesVersion { get; set; }

        public EasUploadDateTime EasUploadDateTime { get; set; }
    }
}
