﻿using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.McaContracts
{
    public class McaDevolvedContract
    {
        public string McaGlaShortCode { get; set; }

        public int Ukprn { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
