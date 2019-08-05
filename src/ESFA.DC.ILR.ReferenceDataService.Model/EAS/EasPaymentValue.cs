namespace ESFA.DC.ILR.ReferenceDataService.Model.EAS
{
    public struct EasPaymentValue
    {
        public EasPaymentValue(decimal? paymentValue, int? devolvedAraSof)
        {
            PaymentValue = paymentValue;
            DevolvedAreaSof = devolvedAraSof;
        }

        public decimal? PaymentValue { get; set; }

        public int? DevolvedAreaSof { get; set; }
    }
}
