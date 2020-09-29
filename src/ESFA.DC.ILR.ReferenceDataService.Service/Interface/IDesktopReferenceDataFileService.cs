using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Interface
{
    public interface IDesktopReferenceDataFileService
    {
        Task ProcessAsync(IReferenceDataContext context, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken);
    }
}
