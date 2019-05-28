using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class LarsStandardReferenceDataMapper : IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>>
    {
        public IReadOnlyCollection<LARSStandard> Retrieve(IReadOnlyCollection<int> input, DesktopReferenceDataRoot referenceData)
        {
            return referenceData.LARSStandards.Where(l => input.Contains(l.StandardCode)).ToList();
        }
    }
}
