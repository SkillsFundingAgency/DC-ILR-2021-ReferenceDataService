using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys
{
    public struct LARSFrameworkKey
    {
        public LARSFrameworkKey(string learnAimRef, LARSFramework larsFramework)
        {
            LearnAimRef = learnAimRef;
            LARSFramework = larsFramework;
        }

        public string LearnAimRef { get; }

        public int FworkCode => LARSFramework.FworkCode;

        public int ProgType => LARSFramework.ProgType;

        public int PwayCode => LARSFramework.PwayCode;

        public LARSFramework LARSFramework { get; }
    }
}
