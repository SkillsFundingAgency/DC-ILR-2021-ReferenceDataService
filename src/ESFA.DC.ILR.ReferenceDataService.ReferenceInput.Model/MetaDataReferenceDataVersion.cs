using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataReferenceDataVersion
    {
        public MetaDataReferenceDataVersion()
        {
            MetaDataMetaData = new HashSet<MetaDataMetaData>();
        }

        public int Id { get; set; }
        public int? CampusIdentifierVersionId { get; set; }
        public int? CoFversionId { get; set; }
        public int? DevolvedPostcodesVersionId { get; set; }
        public int? EasUploadDateTimeId { get; set; }
        public int? EmployersId { get; set; }
        public int? HmppPostcodesVersionId { get; set; }
        public int? LarsVersionId { get; set; }
        public int? OrganisationsVersionId { get; set; }
        public int? PostcodeFactorsVersionId { get; set; }
        public int? PostcodesVersionId { get; set; }

        public virtual MetaDataCampusIdentifierVersion CampusIdentifierVersion { get; set; }
        public virtual MetaDataCoFversion CoFversion { get; set; }
        public virtual MetaDataDevolvedPostcodesVersion DevolvedPostcodesVersion { get; set; }
        public virtual MetaDataEasUploadDateTime EasUploadDateTime { get; set; }
        public virtual MetaDataEmployersVersion Employers { get; set; }
        public virtual MetaDataHmppPostcodesVersion HmppPostcodesVersion { get; set; }
        public virtual MetaDataLarsVersion LarsVersion { get; set; }
        public virtual MetaDataOrganisationsVersion OrganisationsVersion { get; set; }
        public virtual MetaDataPostcodeFactorsVersion PostcodeFactorsVersion { get; set; }
        public virtual MetaDataPostcodesVersion PostcodesVersion { get; set; }
        public virtual ICollection<MetaDataMetaData> MetaDataMetaData { get; set; }
    }
}
