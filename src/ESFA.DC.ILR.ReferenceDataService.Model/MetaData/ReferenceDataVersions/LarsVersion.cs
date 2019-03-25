namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions
{
    public struct LarsVersion
    {
        public LarsVersion(string version)
        {
            Version = version;
        }

        public string Version { get; set; }
    }
}
