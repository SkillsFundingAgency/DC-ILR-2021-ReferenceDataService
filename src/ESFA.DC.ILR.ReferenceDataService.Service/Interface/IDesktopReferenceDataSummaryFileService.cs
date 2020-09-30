using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Interface
{
    public interface IDesktopReferenceDataSummaryFileService
    {
        Task ProcessAync(string container, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken);
    }
}
