using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputEFMapper
    {
        Task<object> Map(IReferenceDataContext referenceDataContext, DesktopReferenceDataRoot desktopReferenceDataRoot, CancellationToken cancellationToken);
    }
}
