namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IReferenceDataContext
    {
        string FileReference { get; set; }

        string OriginalFileReference { get; set; }

        string Container { get; }

        string InputReferenceDataFileKey { get; }

        string OutputReferenceDataFileKey { get; }

        string FrmReferenceDataFileKey { get; }

        string Task { get; }

        int ReturnPeriod { get; set; }

        string ValidationMessagesFileReference { get; }

        string DesktopReferenceDataStoragePath { get; }

        int Ukprn { get; set; }
    }
}
