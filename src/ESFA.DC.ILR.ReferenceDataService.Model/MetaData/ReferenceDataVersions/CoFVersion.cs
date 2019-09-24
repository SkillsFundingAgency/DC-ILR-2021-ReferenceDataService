namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions
{
    public struct CoFVersion
    {
        public CoFVersion(string version)
        {
            Version = version;
        }

        public string Version { get; set; }
    }
}
