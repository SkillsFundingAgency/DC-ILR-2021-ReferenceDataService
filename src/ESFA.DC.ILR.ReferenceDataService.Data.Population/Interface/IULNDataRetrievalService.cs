using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.ULN;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IULNDataRetrievalService
    {
        Task<IReadOnlyCollection<ULN>> RetrieveAsync(IReadOnlyCollection<long> ulns, CancellationToken cancellationToken);
    }
}
