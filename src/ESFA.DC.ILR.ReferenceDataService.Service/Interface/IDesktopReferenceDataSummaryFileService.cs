using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Interface
{
    public interface IDesktopReferenceDataSummaryFileService
    {
        Task ProcessAync(IDesktopReferenceDataContext context, CancellationToken cancellationToken);
    }
}
