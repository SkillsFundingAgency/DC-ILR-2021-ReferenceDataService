using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Interface
{
    public interface IDesktopReferenceDataTask
    {
        Task ExecuteAsync(IDesktopReferenceDataContext context, CancellationToken cancellationToken);
    }
}
