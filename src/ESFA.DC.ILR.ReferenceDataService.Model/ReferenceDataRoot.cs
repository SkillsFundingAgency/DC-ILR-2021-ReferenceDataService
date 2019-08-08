using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.EAS;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;

namespace ESFA.DC.ILR.ReferenceDataService.Model
{
    public class ReferenceDataRoot
    {
        public MetaData.MetaData MetaDatas { get; set; }

        public IReadOnlyCollection<ApprenticeshipEarningsHistory> AppsEarningsHistories { get; set; }

        public IReadOnlyCollection<EasFundingLine> EasFundingLines { get; set; }

        public IReadOnlyCollection<Employer> Employers { get; set; }

        public IReadOnlyCollection<EPAOrganisation> EPAOrganisations { get; set; }

        public IReadOnlyCollection<FcsContractAllocation> FCSContractAllocations { get; set; }

        public IReadOnlyCollection<LARSLearningDelivery> LARSLearningDeliveries { get; set; }

        public IReadOnlyCollection<LARSStandard> LARSStandards { get; set; }

        public IReadOnlyCollection<Organisation> Organisations { get; set; }

        public IReadOnlyCollection<Postcode> Postcodes { get; set; }

        public DevolvedPostcodes DevolvedPostocdes { get; set; }

        public IReadOnlyCollection<long> ULNs { get; set; }
    }
}
