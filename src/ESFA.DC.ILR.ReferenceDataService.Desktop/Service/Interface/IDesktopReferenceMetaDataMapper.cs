using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IDesktopReferenceMetaDataMapper
    {
        MetaData Retrieve(DesktopReferenceDataRoot referenceData);
    }
}
