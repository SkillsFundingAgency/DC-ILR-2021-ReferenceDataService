using System;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IDesktopReferenceDataContext
    {
        long JobId { get; }

        string Container { get; }

        string CollectionName { get; }

        DateTime SubmissionDateTimeUTC { get; }

        string FISReferenceDataVersion { get; }
    }
}
