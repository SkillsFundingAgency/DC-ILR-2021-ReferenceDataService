namespace ESFA.DC.ILR.ReferenceDataService.Model.CsvModels
{
    public class ValidationMessagesModel
    {
        public string RuleName { get; set; }

        public string ErrorMessage { get; set; }

        public string Severity { get; set; }

        public bool? EnabledSLD { get; set; }

        public bool? EnabledDesktop { get; set; }
    }
}
