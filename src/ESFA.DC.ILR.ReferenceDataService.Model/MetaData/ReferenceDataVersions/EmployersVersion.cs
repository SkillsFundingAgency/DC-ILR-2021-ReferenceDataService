namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions
{
    public struct EmployersVersion
    {
        public EmployersVersion(string version)
        {
            Version = version;
        }

        public string Version { get; set; }
    }
}
