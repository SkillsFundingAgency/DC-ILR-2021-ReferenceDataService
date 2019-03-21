using System;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IMessageMapperService
    {
        MapperData MapFromMessage(IMessage message);
    }
}
