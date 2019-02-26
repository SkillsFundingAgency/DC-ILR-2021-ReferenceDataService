using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.FCS;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IFCSDataRetrievalService
    {
        Task<IReadOnlyDictionary<string, FcsContractAllocation>> RetrieveAsync(int ukprn, CancellationToken cancellationToken);
    }
}
