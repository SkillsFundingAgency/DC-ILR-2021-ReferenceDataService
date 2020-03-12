using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IReferenceInputTruncator
    {
        void TruncateReferenceData(IInputReferenceDataContext inputReferenceDataContext);
    }
}
