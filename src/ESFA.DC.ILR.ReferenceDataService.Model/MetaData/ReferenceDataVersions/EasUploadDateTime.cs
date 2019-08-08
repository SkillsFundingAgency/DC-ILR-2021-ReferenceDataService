using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions
{
    public struct EasUploadDateTime
    {
        public EasUploadDateTime(DateTime? uploadDateTime)
        {
            UploadDateTime = uploadDateTime;
        }

        public DateTime? UploadDateTime { get; set; }
    }
}
