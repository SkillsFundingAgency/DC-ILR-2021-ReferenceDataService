using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataReferenceDataVersion
    {
        public MetaDataReferenceDataVersion()
        {
            MetaDataMetaDatas = new HashSet<MetaDataMetaData>();
        }

        public int Id { get; set; }
        public int? CampusIdentifierVersion_Id { get; set; }
        public int? CoFVersion_Id { get; set; }
        public int? DevolvedPostcodesVersion_Id { get; set; }
        public int? EasUploadDateTime_Id { get; set; }
        public int? Employers_Id { get; set; }
        public int? HmppPostcodesVersion_Id { get; set; }
        public int? LarsVersion_Id { get; set; }
        public int? OrganisationsVersion_Id { get; set; }
        public int? PostcodeFactorsVersion_Id { get; set; }
        public int? PostcodesVersion_Id { get; set; }

        public virtual MetaDataCampusIdentifierVersion CampusIdentifierVersion_ { get; set; }
        public virtual MetaDataCoFversion CoFVersion_ { get; set; }
        public virtual MetaDataDevolvedPostcodesVersion DevolvedPostcodesVersion_ { get; set; }
        public virtual MetaDataEasUploadDateTime EasUploadDateTime_ { get; set; }
        public virtual MetaDataEmployersVersion Employers_ { get; set; }
        public virtual MetaDataHmppPostcodesVersion HmppPostcodesVersion_ { get; set; }
        public virtual MetaDataLarsVersion LarsVersion_ { get; set; }
        public virtual MetaDataOrganisationsVersion OrganisationsVersion_ { get; set; }
        public virtual MetaDataPostcodeFactorsVersion PostcodeFactorsVersion_ { get; set; }
        public virtual MetaDataPostcodesVersion PostcodesVersion_ { get; set; }
        public virtual ICollection<MetaDataMetaData> MetaDataMetaDatas { get; set; }
    }
}
