namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IInputReferenceDataContext
    {
        string InputReferenceDataFileKey { get; set; }

        string Container { get; set; }

        string ConnectionString { get; set;  }
    }
}
