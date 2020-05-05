using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_ReferenceDataVersion
    {
        public MetaData_ReferenceDataVersion()
        {
            MetaData_MetaDatas = new HashSet<MetaData_MetaData>();
        }

        public int Id { get; set; }
        public int? CampusIdentifierVersion_Id { get; set; }
        public int? CoFVersion_Id { get; set; }
        public int? DevolvedPostcodesVersion_Id { get; set; }
        public int? EasFileDetails_Id { get; set; }
        public int? EmployersVersion_Id { get; set; }
        public int? HmppPostcodesVersion_Id { get; set; }
        public int? LarsVersion_Id { get; set; }
        public int? OrganisationsVersion_Id { get; set; }
        public int? PostcodeFactorsVersion_Id { get; set; }
        public int? PostcodesVersion_Id { get; set; }

        public virtual MetaData_CampusIdentifierVersion CampusIdentifierVersion_ { get; set; }
        public virtual MetaData_CoFVersion CoFVersion_ { get; set; }
        public virtual MetaData_DevolvedPostcodesVersion DevolvedPostcodesVersion_ { get; set; }
        public virtual MetaData_EasFileDetails EasFileDetails_ { get; set; }
        public virtual MetaData_EmployersVersion EmployersVersion_ { get; set; }
        public virtual MetaData_HmppPostcodesVersion HmppPostcodesVersion_ { get; set; }
        public virtual MetaData_LarsVersion LarsVersion_ { get; set; }
        public virtual MetaData_OrganisationsVersion OrganisationsVersion_ { get; set; }
        public virtual MetaData_PostcodeFactorsVersion PostcodeFactorsVersion_ { get; set; }
        public virtual MetaData_PostcodesVersion PostcodesVersion_ { get; set; }
        public virtual ICollection<MetaData_MetaData> MetaData_MetaDatas { get; set; }
    }
}
