namespace ESFA.DC.ILR.ReferenceDataService.Interfaces.Exception
{
    public class ReferenceDataServiceFailureException : System.Exception
    {
        public ReferenceDataServiceFailureException()
        {
        }

        public ReferenceDataServiceFailureException(string errorMessage)
            : base(errorMessage)
        {
        }

        public ReferenceDataServiceFailureException(string errorMessage, System.Exception innerException)
        : base(errorMessage, innerException)
        {
        }
    }
}
