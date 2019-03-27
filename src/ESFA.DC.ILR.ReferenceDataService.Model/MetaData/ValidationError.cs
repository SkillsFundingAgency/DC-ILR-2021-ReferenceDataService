namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public class ValidationError
    {
        public enum SeverityLevel
        {
            Error,
            Warning,

            /// <summary>
            /// File is determined to be unreadable, and not able to validated.
            /// </summary>
            Fail
        }

        public string RuleName { get; set; }

        public SeverityLevel Severity { get; set; }

        public string Message { get; set; }
    }
}
