using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.FRM;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface
{
    public interface IFrmReferenceDataRepositoryService
    {
        Task<IReadOnlyCollection<FrmLearner>> RetrieveFrm06ReferenceDataAsync(long ukprn, CancellationToken cancellationToken);
    }
}
