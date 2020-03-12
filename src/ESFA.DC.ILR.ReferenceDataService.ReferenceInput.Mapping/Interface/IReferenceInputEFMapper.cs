using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IReferenceInputEFMapper
    {
        TTarget MapByType<TSource, TTarget>(TSource source);

        IEFReferenceInputDataRoot Map(DesktopReferenceDataRoot desktopReferenceDataRoot);
    }
}
