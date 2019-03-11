namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public struct ReferenceDataVersion
    {
        public ReferenceDataVersion(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; set; }

        public string Version { get; set; }
    }
}
