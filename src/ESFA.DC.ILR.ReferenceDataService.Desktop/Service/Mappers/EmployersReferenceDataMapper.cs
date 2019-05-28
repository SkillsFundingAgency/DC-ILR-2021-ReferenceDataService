using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers
{
    public class EmployersReferenceDataMapper : IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>>
    {
        public IReadOnlyCollection<Employer> Retrieve(IReadOnlyCollection<int> input, DesktopReferenceDataRoot referenceData)
        {
            return referenceData.Employers.Where(e => input.Contains(e.ERN)).ToList();
        }
    }
}
