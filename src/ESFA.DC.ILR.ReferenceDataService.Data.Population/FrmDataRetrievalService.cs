using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.FRM;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class FrmDataRetrievalService : IFrmDataRetrievalService
    {
        private readonly IFrmReferenceDataRepositoryService _frmReferenceDataRepositoryService;

        public FrmDataRetrievalService(IFrmReferenceDataRepositoryService frmReferenceDataRepositoryService)
        {
            _frmReferenceDataRepositoryService = frmReferenceDataRepositoryService;
        }

        public async Task<FrmReferenceData> RetrieveAsync(long ukprn, CancellationToken cancellationToken)
        {
            return new FrmReferenceData
            {
                Frm06Learners = await _frmReferenceDataRepositoryService.RetrieveFrm06ReferenceDataAsync(ukprn, cancellationToken)
            };
        }
    }
}
