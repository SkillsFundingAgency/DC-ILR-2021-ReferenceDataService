namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions
{
    public struct OrganisationsVersion
    {
        public OrganisationsVersion(string version)
        {
            Version = version;
        }

        public string Version { get; set; }
    }
}
