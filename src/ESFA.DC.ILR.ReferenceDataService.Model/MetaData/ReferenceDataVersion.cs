using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public class ReferenceDataVersion
    {
        public CoFVersion CoFVersion { get; set; }

        public CampusIdentifierVersion CampusIdentifierVersion { get; set; }

        public EmployersVersion Employers { get; set; }

        public LarsVersion LarsVersion { get; set; }

        public OrganisationsVersion OrganisationsVersion { get; set; }

        public PostcodesVersion PostcodesVersion { get; set; }

        public DevolvedPostcodesVersion DevolvedPostcodesVersion { get; set; }

        public HmppPostcodesVersion HmppPostcodesVersion { get; set; }

        public PostcodeFactorsVersion PostcodeFactorsVersion { get; set; }

        public EasUploadDateTime EasUploadDateTime { get; set; }
    }
}
