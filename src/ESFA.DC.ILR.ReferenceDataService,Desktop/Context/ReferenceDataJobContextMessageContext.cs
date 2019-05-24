using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.Desktop.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Context
{
    public class ReferenceDataJobContextMessageContext : IReferenceDataContext
    {
        private readonly IDesktopContext _desktopContext;

        public ReferenceDataJobContextMessageContext(IDesktopContext desktopContext)
        {
            _desktopContext = desktopContext;
        }

        public string FileReference
        {
            get => _desktopContext.KeyValuePairs[ILRContextKeys.Filename].ToString();
            set => _desktopContext.KeyValuePairs[ILRContextKeys.Filename] = value;
        }

        public string OriginalFileReference
        {
            get => _desktopContext.KeyValuePairs[ILRContextKeys.OriginalFilename].ToString();
            set => _desktopContext.KeyValuePairs[ILRContextKeys.OriginalFilename] = value;
        }

        public string Container => _desktopContext.KeyValuePairs[ILRContextKeys.Container].ToString();

        public string OutputReferenceDataFileKey => _desktopContext.KeyValuePairs[ILRContextKeys.IlrReferenceData].ToString();

        public string Task { get; }
    }
}
