﻿using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSStandardValidity
    {
        public string ValidityCategory { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? LastNewStartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
