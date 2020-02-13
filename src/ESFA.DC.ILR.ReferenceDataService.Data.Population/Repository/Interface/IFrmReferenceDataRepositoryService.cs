using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.FRM;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IFrmReferenceDataRepositoryService
    {
        Task<IReadOnlyCollection<Frm06Learner>> RetrieveFrm06ReferenceDataAsync(long ukprn, CancellationToken cancellationToken);
    }
}
