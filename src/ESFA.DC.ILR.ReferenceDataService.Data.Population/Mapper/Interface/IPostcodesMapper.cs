﻿using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface
{
    public interface IPostcodesMapper
    {
        IReadOnlyCollection<string> MapPostcodesFromMessage(IMessage input);
    }
}
