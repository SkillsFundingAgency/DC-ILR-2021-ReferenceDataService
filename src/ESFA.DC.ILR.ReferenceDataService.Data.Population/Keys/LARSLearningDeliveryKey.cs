namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys
{
    public struct LARSLearningDeliveryKey
    {
        public LARSLearningDeliveryKey(string learnAimRef, int? fworkCode, int? progType, int? pwayCode)
        {
            LearnAimRef = learnAimRef;
            FworkCode = fworkCode;
            ProgType = progType;
            PwayCode = pwayCode;
        }

        public string LearnAimRef { get; }

        public int? FworkCode { get; }

        public int? ProgType { get; }

        public int? PwayCode { get; }
    }
}
