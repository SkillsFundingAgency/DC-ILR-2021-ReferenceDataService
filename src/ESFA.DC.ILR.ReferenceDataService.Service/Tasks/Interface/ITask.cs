﻿using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tasks.Interface
{
    public interface ITask
    {
        Task ExecuteAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken);
    }
}
