﻿using ESFA.DC.ILR.ReferenceDataService.Model.LARS.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSStandardApprenticeshipFunding : AbstractLARSApprenticeshipFunding
    {
        public int ProgType { get; set; }

        public int? PwayCode { get; set; }
    }
}
