using System;
using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Constants;

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

        public string DesktopInputReferenceDataFileKey => _desktopContext.KeyValuePairs[ILRContextKeys.ReferenceDataFilename].ToString();

        public string OutputIlrReferenceDataFileKey => _desktopContext.KeyValuePairs[ILRContextKeys.IlrReferenceData].ToString();

        public string FrmReferenceDataFileKey => throw new NotImplementedException();

        public string LearnerReferenceDataFileKey => throw new NotImplementedException();

        public string Task { get; }

        public int ReturnPeriod
        {
            get => int.Parse(_desktopContext.KeyValuePairs[ILRContextKeys.ReturnPeriod].ToString());
            set => _desktopContext.KeyValuePairs[ILRContextKeys.ReturnPeriod] = value;
        }

        public string ValidationMessagesFileReference => _desktopContext.KeyValuePairs[ReferenceDataContextKeys.ValidationMessagesFileReferenceKey].ToString();

        public int Ukprn
        {
            get => int.Parse(_desktopContext.KeyValuePairs[ILRContextKeys.Ukprn].ToString());
            set => _desktopContext.KeyValuePairs[ILRContextKeys.Ukprn] = value;
        }
    }
}
