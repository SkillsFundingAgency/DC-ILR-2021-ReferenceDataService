using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Context
{
    public class InputReferenceDataContext : IInputReferenceDataContext
    {
        public string InputReferenceDataFileKey { get; set; }

        public string Container { get; set; }

        public string ConnectionString { get; set; }
    }
}
