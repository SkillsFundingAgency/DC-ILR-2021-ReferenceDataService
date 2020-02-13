using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.FRM;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IFrmDataRetrievalService
    {
        Task<FrmReferenceData> RetrieveAsync(long ukprn, CancellationToken cancellationToken);
    }
}
