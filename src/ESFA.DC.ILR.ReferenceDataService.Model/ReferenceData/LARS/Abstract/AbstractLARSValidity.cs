using System;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS.Abstract
{
    public class AbstractLARSValidity : AbstractTimeBoundedEntity
    {
        public string ValidityCategory { get; set; }

        public DateTime StartDate => EffectiveFrom;

        public DateTime? LastNewStartDate { get; set; }

        public DateTime? EndDate => EffectiveTo;
    }
}
