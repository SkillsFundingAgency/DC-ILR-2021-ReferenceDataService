using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IReferenceDataPopulationService
    {
        Task<ReferenceDataRoot> PopulateAsync(IReferenceDataContext referenceDataContext, IMessage message, CancellationToken cancellationToken);
    }
}
