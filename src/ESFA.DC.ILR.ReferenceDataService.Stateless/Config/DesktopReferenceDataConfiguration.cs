using ESFA.DC.ILR.ReferenceDataService.Interfaces.Config;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Config
{
    public class DesktopReferenceDataConfiguration : IDesktopReferenceDataConfiguration
    {
        public string DesktopReferenceDataFilePreFix { get; set; }

        public string DesktopReferenceDataStoragePath { get; set; }
    }
}
