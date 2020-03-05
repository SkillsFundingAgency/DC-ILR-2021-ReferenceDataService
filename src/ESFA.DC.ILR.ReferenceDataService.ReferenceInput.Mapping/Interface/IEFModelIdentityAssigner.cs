using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IEFModelIdentityAssigner
    {
        void AssignIds(IEFReferenceInputDataRoot inputData);
    }
}
