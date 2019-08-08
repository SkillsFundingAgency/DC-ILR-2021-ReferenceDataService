using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class DevolvedPostcodesReferenceDataMapper : IDesktopReferenceDataMapper<IReadOnlyCollection<string>, DevolvedPostcodes>
    {
        public DevolvedPostcodes Retrieve(IReadOnlyCollection<string> input, DesktopReferenceDataRoot referenceData)
        {
            return referenceData.DevolvedPostocdes;
        }
    }
}
