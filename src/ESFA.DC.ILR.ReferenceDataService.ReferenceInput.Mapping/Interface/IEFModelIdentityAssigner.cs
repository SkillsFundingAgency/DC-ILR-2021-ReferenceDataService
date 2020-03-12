using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface
{
    public interface IEFModelIdentityAssigner
    {
        void AssignIdsByType<T>(IEnumerable<T> source);
    }
}
