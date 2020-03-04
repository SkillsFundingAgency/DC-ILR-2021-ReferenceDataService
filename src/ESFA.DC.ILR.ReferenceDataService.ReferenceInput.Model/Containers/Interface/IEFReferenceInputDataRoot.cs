using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface
{
    public interface IEFReferenceInputDataRoot
    {
        LARS_LARSVersion Lars_LarsVersion { get; set; }
        ICollection<LARS_LARSStandard> Lars_LarsStandards { get; set; }
        ICollection<LARS_LARSLearningDelivery> Lars_LarsLearningDeliveries { get; set; }
        ICollection<LARS_LARSFrameworkDesktop> Lars_LarsFrameworkDesktops { get; set; }
        ICollection<LARS_LARSFrameworkAim> Lars_LarsFrameworkAims { get; set; }
    }
}
