﻿namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model
{
    public partial class Rule
    {
        public string Rulename { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public bool? Online { get; set; }
        public bool? Desktop { get; set; }
    }
}
