using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface
{
    public interface IDesktopReferenceDataPopulationService
    {
        Task<DesktopReferenceDataRoot> PopulateAsync(CancellationToken cancellationToken);
    }
}
