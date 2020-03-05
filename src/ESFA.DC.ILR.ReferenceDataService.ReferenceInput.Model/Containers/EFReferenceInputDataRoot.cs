using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers
{
    public class EFReferenceInputDataRoot : IEFReferenceInputDataRoot
    {
        public LARS_LARSVersion Lars_LarsVersion { get; set; }
        public ICollection<LARS_LARSStandard> Lars_LarsStandards { get; set; }
        public ICollection<LARS_LARSLearningDelivery> Lars_LarsLearningDeliveries { get; set; }
        public ICollection<LARS_LARSFrameworkDesktop> Lars_LarsFrameworkDesktops { get; set; }
        public ICollection<LARS_LARSFrameworkAim> Lars_LarsFrameworkAims { get; set; }
    }
}
