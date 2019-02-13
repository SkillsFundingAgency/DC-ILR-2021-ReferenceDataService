namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.FCS
{
    public class FcsContractDeliverable
    {
        public int? DeliverableCode { get; set; }

        public string DeliverableDescription { get; set; }

        public string ExternalDeliverableCode { get; set; }

        public decimal? UnitCost { get; set; }

        public int? PlannedVolume { get; set; }

        public decimal? PlannedValue { get; set; }
    }
}
