using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class PostcodesReferenceDataMapper : IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>>
    {
        public IReadOnlyCollection<Postcode> Retrieve(IReadOnlyCollection<string> input, DesktopReferenceDataRoot referenceData)
        {
            return referenceData.Postcodes.Where(p => input.Contains(p.PostCode)).ToList();
        }
    }
}
