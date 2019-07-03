﻿using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface
{
    public interface IUkprnsMapper
    {
        IReadOnlyCollection<int> MapUKPRNsFromMessage(IMessage input);
    }
}
