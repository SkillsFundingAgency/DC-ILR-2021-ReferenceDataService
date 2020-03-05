using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface
{
    public interface IDesktopMetaDataRetrievalService
    {
        Task<MetaData> RetrieveAsync(CancellationToken cancellationToken);
    }
}
