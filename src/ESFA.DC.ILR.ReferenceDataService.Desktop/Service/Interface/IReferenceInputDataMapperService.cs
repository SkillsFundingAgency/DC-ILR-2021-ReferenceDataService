using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IReferenceInputDataMapperService
    {
        Task<IReadOnlyCollection<T>> MapReferenceDataByType<T>(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken);

        Task<DesktopReferenceDataRoot> MapReferenceData(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken);
    }
}
