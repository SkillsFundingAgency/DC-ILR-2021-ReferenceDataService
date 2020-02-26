﻿using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsstandardValidity
    {
        public int Id { get; set; }
        public string ValidityCategory { get; set; }
        public DateTime? LastNewStartDate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LarsLarsstandardId { get; set; }

        public virtual LarsLarsstandard LarsLarsstandard { get; set; }
    }
}
