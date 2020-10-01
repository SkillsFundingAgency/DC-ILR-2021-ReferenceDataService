namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IReferenceDataSummaryStatistics
    {
        string DataSource { get;  }

        int NumberOfRecords { get;  }
    }
}
