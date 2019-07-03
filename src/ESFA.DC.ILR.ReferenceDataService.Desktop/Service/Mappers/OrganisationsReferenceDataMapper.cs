using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class OrganisationsReferenceDataMapper : IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>>
    {
        public IReadOnlyCollection<Organisation> Retrieve(IReadOnlyCollection<int> input, DesktopReferenceDataRoot referenceData)
        {
            return referenceData.Organisations.Where(o => input.Contains(o.UKPRN)).ToList();
        }
    }
}
