namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions
{
    public struct PostcodesVersion
    {
        public PostcodesVersion(string version)
        {
            Version = version;
        }

        public string Version { get; set; }
    }
}
