﻿namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Model
{
    public class EasSubmissionDecodedValue
    {
        public string FundingLine { get; set; }

        public string AdjustmentName { get; set; }

        public string PaymentName { get; set; }

        public int Period { get; set; }

        public decimal? PaymentValue { get; set; }

        public int DevolvedAreaSof { get; set; }
    }
}
