using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IReferenceMetaDataService
    {
        Task<MetaData> Retrieve(CancellationToken cancellationToken);
    }
}
