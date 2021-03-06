﻿using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface
{
    public interface IDesktopReferenceDataRepositoryService<T>
    {
        Task<T> RetrieveAsync(CancellationToken cancellationToken);
    }
}
