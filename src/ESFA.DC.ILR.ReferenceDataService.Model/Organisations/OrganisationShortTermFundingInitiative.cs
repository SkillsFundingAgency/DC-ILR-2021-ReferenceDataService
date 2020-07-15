using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisations
{
    public class OrganisationShortTermFundingInitiative : AbstractTimeBoundedEntity
    {
        public long UKPRN { get; set; }

        public string LdmCode { get; set; }

        public string Reason { get; set; }
    }
}
