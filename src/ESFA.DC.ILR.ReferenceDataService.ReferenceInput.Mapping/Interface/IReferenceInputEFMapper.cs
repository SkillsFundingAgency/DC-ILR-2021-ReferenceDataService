using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IReferenceInputEFMapper
    {
        IEFReferenceInputDataRoot Map(DesktopReferenceDataRoot desktopReferenceDataRoot);
    }
}
