using System;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IReferenceDataContext
    {
        long JobId { get; }

        string FileReference { get; set; }

        string OriginalFileReference { get; set; }

        string Container { get; }

        string DesktopInputReferenceDataFileKey { get; }

        string OutputIlrReferenceDataFileKey { get; }

        string FrmReferenceDataFileKey { get; }

        string FISReferenceDataVersion { get; }

        string LearnerReferenceDataFileKey { get; }

        string Task { get; }

        int ReturnPeriod { get; set; }

        string ValidationMessagesFileReference { get; }

        int Ukprn { get; set; }

        string CollectionName { get; }

        DateTime SubmissionDateTimeUTC { get; }
    }
}
