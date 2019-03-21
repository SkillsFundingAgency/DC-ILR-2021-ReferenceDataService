using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface
{
    public interface IUlnMapper
    {
        IReadOnlyCollection<long> MapPostcodesFromMessage(IMessage input);
    }
}