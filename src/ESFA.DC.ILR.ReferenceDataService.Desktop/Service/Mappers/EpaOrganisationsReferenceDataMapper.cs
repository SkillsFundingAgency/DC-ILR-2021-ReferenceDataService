using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class EpaOrganisationsReferenceDataMapper : IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>>
    {
        public IReadOnlyCollection<EPAOrganisation> Retrieve(IReadOnlyCollection<string> input, DesktopReferenceDataRoot referenceData)
        {
            return referenceData.EPAOrganisations.Where(e => input.Contains(e.ID)).ToList();
        }
    }
}
