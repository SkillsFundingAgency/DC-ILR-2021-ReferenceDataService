namespace ESFA.DC.ILR.ReferenceDataService.Model.EAS
{
    public struct EasPaymentValue
    {
        public EasPaymentValue(decimal? paymentValue, int? devolvedAraSofs)
        {
            PaymentValue = paymentValue;
            DevolvedAreaSofs = devolvedAraSofs;
        }

        public decimal? PaymentValue { get; set; }

        public int? DevolvedAreaSofs { get; set; }
    }
}
