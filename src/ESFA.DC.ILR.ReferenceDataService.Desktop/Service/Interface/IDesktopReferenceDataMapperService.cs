using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface
{
    public interface IDesktopReferenceDataMapperService
    {
        Task<ReferenceDataRoot> MapReferenceData(IReferenceDataContext referenceDataContext, MapperData mapperData, CancellationToken cancellationToken);
    }
}
