using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.ULNs;

namespace ESFA.DC.ILR.ReferenceDataService.Model
{
    public class ReferenceDataRoot
    {
        public MetaData.MetaData MetaDatas { get; set; }

        public IReadOnlyDictionary<int, Employer> Employers { get; set; }

        public IReadOnlyDictionary<string, List<EPAOrganisation>> EPAOrganisations { get; set; }

        public IReadOnlyDictionary<string, FcsContractAllocation> FCSContractAllocations { get; set; }

        public IReadOnlyDictionary<string, LARSLearningDelivery> LARSLearningDeliveries { get; set; }

        public IReadOnlyDictionary<int, LARSStandard> LARSStandards { get; set; }

        public IReadOnlyDictionary<int, Organisation> Organisations { get; set; }

        public IReadOnlyDictionary<string, Postcode> Postcodes { get; set; }

        public IReadOnlyCollection<ULN> ULNs { get; set; }
    }
}
