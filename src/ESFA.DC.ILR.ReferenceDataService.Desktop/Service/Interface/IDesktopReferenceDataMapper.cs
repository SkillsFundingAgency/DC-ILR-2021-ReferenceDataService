using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IDesktopReferenceDataMapper<TIn, TOut>
    {
        TOut Map(TIn input, DesktopReferenceDataRoot referenceData);
    }
}
