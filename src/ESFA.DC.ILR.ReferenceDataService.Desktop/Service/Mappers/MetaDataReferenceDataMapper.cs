using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class MetaDataReferenceDataMapper : IDesktopReferenceMetaDataMapper
    {
        public MetaData Retrieve(DesktopReferenceDataRoot referenceData)
        {
            return referenceData.MetaDatas;
        }
    }
}
