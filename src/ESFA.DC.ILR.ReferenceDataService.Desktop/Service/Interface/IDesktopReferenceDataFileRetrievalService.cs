using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IDesktopReferenceDataFileRetrievalService
    {
        DesktopReferenceDataRoot Retrieve();
    }
}
