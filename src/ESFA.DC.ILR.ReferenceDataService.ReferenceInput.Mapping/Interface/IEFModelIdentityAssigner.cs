using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model.Containers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IEFModelIdentityAssigner
    {
        void AssignIdsByType<T>(IEnumerable<T> source);

        void AssignIds(IEFReferenceInputDataRoot inputData);
    }
}
