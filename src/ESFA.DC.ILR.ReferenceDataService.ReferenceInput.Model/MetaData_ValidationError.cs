namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_ValidationError
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public int Severity { get; set; }
        public string Message { get; set; }
    }
}
